using System.Text.Json;
using AestheticEMR.Core.Models.Aesthetic;
using AestheticEMR.Core.Models.Dental;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Aesthetics;
using AestheticEMR.Core.Services.Dental.Interfaces;
using AestheticEMR.Server.ViewModels.Dental;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DentalController(
    ILogger<DentalController> logger,
    IMapper mapper,
    IDentalService dentalService,
    IAuditService auditService,
    IWebHostEnvironment environment) : BaseApiController(logger, mapper)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private const string DentalUploadFolder = "uploads/dental-imaging";

    // ─── Combined Encounter (single transaction) ────────────────────────────

    [HttpGet("encounter")]
    [ProducesResponseType(typeof(DentalEncounterVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetEncounter([FromQuery] string consultId, [FromQuery] string pno)
    {
        if (string.IsNullOrWhiteSpace(consultId) || string.IsNullOrWhiteSpace(pno))
            return BadRequest("consultId and pno are required.");

        var encounter = dentalService.GetEncounter(consultId, pno);
        if (encounter == null)
            return NotFound();

        var chartVm = _mapper.Map<DentalChartVM>(encounter.Value.Chart);
        chartVm.Dtype = ExpandDtypeForDisplay(chartVm.Dtype);
        chartVm.TeethStatus = ResolveTeethStatus(encounter.Value.Chart);
        chartVm.Orthodontics = DeserializeOrthodontics(encounter.Value.Chart.OrthodonticsJson);
        chartVm.OralExam = DeserializeOralExam(encounter.Value.Chart.OralExamJson);

        return Ok(new DentalEncounterVM
        {
            Chart = chartVm,
            Imaging = _mapper.Map<DentalImagingVM>(encounter.Value.Imaging),
            Consulting = _mapper.Map<DentalConsultingVM>(encounter.Value.Consulting)
        });
    }

    [HttpPost("encounter")]
    [ProducesResponseType(typeof(DentalEncounterVM), StatusCodes.Status200OK)]
    public async Task<IActionResult> SaveEncounter([FromBody] DentalEncounterSaveVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Resolve chart: required for pno/consultId
        if (vm.Chart == null || string.IsNullOrWhiteSpace(vm.Chart.Pno) || string.IsNullOrWhiteSpace(vm.Chart.ConsultId))
        {
            AddModelError("Patient (PNo and ConsultId) are required.");
            return BadRequest(ModelState);
        }

        // Services is required on the consulting record
        if (vm.Consulting != null && string.IsNullOrWhiteSpace(vm.Consulting.Services))
        {
            AddModelError("Services is required.", nameof(vm.Consulting.Services));
            return BadRequest(ModelState);
        }

        // Findings is required on the imaging record
        if (vm.Imaging != null && string.IsNullOrWhiteSpace(vm.Imaging.Findings))
        {
            AddModelError("Findings is required.", nameof(vm.Imaging.Findings));
            return BadRequest(ModelState);
        }

        var chart = _mapper.Map<HDentalTreat>(vm.Chart);
        chart.Dtype = NormalizeDtypeForStorage(vm.Chart.Dtype);
        chart.TeethStatusJson = SerializeTeethStatus(vm.Chart.TeethStatus);
        chart.OrthodonticsJson = SerializeOrthodontics(vm.Chart.Orthodontics);
        chart.OralExamJson = SerializeOralExam(vm.Chart.OralExam);

        // Build empty stubs when imaging/consulting are omitted
        var imaging = vm.Imaging != null
            ? _mapper.Map<DentalImaging>(vm.Imaging)
            : new DentalImaging { Pno = chart.Pno, ConsultId = chart.ConsultId, ImagingDate = DateTime.UtcNow };

        HConsulting consulting;
        if (vm.Consulting != null)
        {
            consulting = _mapper.Map<HConsulting>(vm.Consulting);
        }
        else
        {
            consulting = new HConsulting
            {
                ConsultId = chart.ConsultId,
                PNo = chart.Pno,
                ClientCat = "PRIVATE",
                TreatedBy = GetCurrentUserEmpId() ?? GetCurrentUserId(),
                CDate = DateTime.UtcNow
            };
        }

        consulting.ConsultId = chart.ConsultId;
        consulting.PNo = chart.Pno;
        // TreatedBy is always the logged-in user's empId
        consulting.TreatedBy = GetCurrentUserEmpId() ?? GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(consulting.ClientCat))
            consulting.ClientCat = "PRIVATE";

        try
        {
            var saved = dentalService.SaveEncounter(chart, imaging, consulting, GetCurrentUserId(), vm.TimeZoneId);

            var auditEventDateTime = ResolveAuditEventDateTime(vm.TimeZoneId);

            await auditService.LogEventAsync(new AuditLog
            {
                TranCode = string.IsNullOrWhiteSpace(saved.Chart.ConsultId) ? "GENERAL" : saved.Chart.ConsultId,
                EventType = chart.Id > 0 ? "Update" : "Create",
                Summary = "Dental encounter saved",
                Details = $"Dental encounter saved for ConsultId '{saved.Chart.ConsultId}' and PNo '{saved.Chart.Pno}'.",
                Severity = "Info",
                EntityType = nameof(HDentalTreat),
                EntityId = (int?)saved.Chart.Id,
                UserId = GetCurrentUserId(),
                PerformedBy = GetCurrentUserId(),
                SourceIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
                Tags = "#dental #encounter #save",
                Status = "Logged",
                EventDateTime = auditEventDateTime
            });

            var savedChartVm = _mapper.Map<DentalChartVM>(saved.Chart);
            savedChartVm.Dtype = ExpandDtypeForDisplay(savedChartVm.Dtype);
            savedChartVm.TeethStatus = ResolveTeethStatus(saved.Chart);
            savedChartVm.Orthodontics = DeserializeOrthodontics(saved.Chart.OrthodonticsJson);
            savedChartVm.OralExam = DeserializeOralExam(saved.Chart.OralExamJson);

            return Ok(new DentalEncounterVM
            {
                Chart = savedChartVm,
                Imaging = _mapper.Map<DentalImagingVM>(saved.Imaging),
                Consulting = _mapper.Map<DentalConsultingVM>(saved.Consulting)
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Encounter save forbidden for consultId {ConsultId}", chart.ConsultId);
            AddModelError(ex.Message);
            return Forbid();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid dental encounter date/time input for consultId {ConsultId}", chart.ConsultId);
            AddModelError(ex.Message);
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving dental encounter for consultId {ConsultId}", chart.ConsultId);
            AddModelError("Unable to save the dental encounter.");
            return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
        }
    }

    // ─── Odontogram / Dental Treatment Chart (HDentalTreat) ─────────────────

    [HttpGet("charts")]
    [ProducesResponseType(typeof(IEnumerable<DentalChartVM>), StatusCodes.Status200OK)]
    public IActionResult GetCharts()
    {
        var chartEntities = dentalService.GetCharts().ToList();
        var result = _mapper.Map<IEnumerable<DentalChartVM>>(chartEntities).ToList();

        for (var i = 0; i < result.Count && i < chartEntities.Count; i++)
        {
            result[i].Dtype = ExpandDtypeForDisplay(result[i].Dtype);
            result[i].TeethStatus = ResolveTeethStatus(chartEntities[i]);
            result[i].Orthodontics = DeserializeOrthodontics(chartEntities[i].OrthodonticsJson);
            result[i].OralExam = DeserializeOralExam(chartEntities[i].OralExamJson);
        }

        return Ok(result);
    }

    [HttpGet("charts/{id:long}")]
    [ProducesResponseType(typeof(DentalChartVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetChart(long id)
    {
        var chart = dentalService.GetChartById(id);
        if (chart == null) return NotFound(id);

        var vm = _mapper.Map<DentalChartVM>(chart);
        vm.Dtype = ExpandDtypeForDisplay(vm.Dtype);
        vm.TeethStatus = ResolveTeethStatus(chart);
        vm.Orthodontics = DeserializeOrthodontics(chart.OrthodonticsJson);
        vm.OralExam = DeserializeOralExam(chart.OralExamJson);
        return Ok(vm);
    }

    [HttpPost("charts")]
    [ProducesResponseType(typeof(DentalChartVM), StatusCodes.Status201Created)]
    public IActionResult CreateChart([FromBody] DentalChartVM vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var entity = _mapper.Map<HDentalTreat>(vm);
        entity.Dtype = NormalizeDtypeForStorage(vm.Dtype);
        entity.TeethStatusJson = SerializeTeethStatus(vm.TeethStatus);
        entity.OrthodonticsJson = SerializeOrthodontics(vm.Orthodontics);
        entity.OralExamJson = SerializeOralExam(vm.OralExam);
        var created = dentalService.AddChart(entity);

        var createdVm = _mapper.Map<DentalChartVM>(created);
        createdVm.Dtype = ExpandDtypeForDisplay(createdVm.Dtype);
        createdVm.TeethStatus = ResolveTeethStatus(created);
        createdVm.Orthodontics = DeserializeOrthodontics(created.OrthodonticsJson);
        createdVm.OralExam = DeserializeOralExam(created.OralExamJson);
        return CreatedAtAction(nameof(GetChart), new { id = created.Id }, createdVm);
    }

    [HttpPut("charts/{id:long}")]
    [ProducesResponseType(typeof(DentalChartVM), StatusCodes.Status200OK)]
    public IActionResult UpdateChart(long id, [FromBody] DentalChartVM vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != vm.Id) return BadRequest("Chart id mismatch");

        try
        {
            var entity = _mapper.Map<HDentalTreat>(vm);
            entity.Dtype = NormalizeDtypeForStorage(vm.Dtype);
            entity.TeethStatusJson = SerializeTeethStatus(vm.TeethStatus);
            entity.OrthodonticsJson = SerializeOrthodontics(vm.Orthodontics);
            entity.OralExamJson = SerializeOralExam(vm.OralExam);
            var updated = dentalService.UpdateChart(entity, GetCurrentUserId());

            var updatedVm = _mapper.Map<DentalChartVM>(updated);
            updatedVm.Dtype = ExpandDtypeForDisplay(updatedVm.Dtype);
            updatedVm.TeethStatus = ResolveTeethStatus(updated);
            updatedVm.Orthodontics = DeserializeOrthodontics(updated.OrthodonticsJson);
            updatedVm.OralExam = DeserializeOralExam(updated.OralExamJson);
            return Ok(updatedVm);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Chart update forbidden {Id}", id);
            AddModelError(ex.Message);
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(id);
        }
    }

    [HttpDelete("charts/{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteChart(long id)
    {
        try
        {
            dentalService.DeleteChart(id, GetCurrentUserId());
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Chart delete forbidden {Id}", id);
            AddModelError(ex.Message);
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(id);
        }
    }

    // ─── Imaging ─────────────────────────────────────────────────────────────

    [HttpGet("imaging")]
    [ProducesResponseType(typeof(IEnumerable<DentalImagingVM>), StatusCodes.Status200OK)]
    public IActionResult GetImaging()
    {
        var records = dentalService.GetImagingRecords();
        return Ok(_mapper.Map<IEnumerable<DentalImagingVM>>(records));
    }

    [HttpGet("imaging/{id:int}")]
    [ProducesResponseType(typeof(DentalImagingVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetImagingById(int id)
    {
        var record = dentalService.GetImagingById(id);
        if (record == null) return NotFound(id);
        return Ok(_mapper.Map<DentalImagingVM>(record));
    }

    [HttpPost("imaging")]
    [ProducesResponseType(typeof(DentalImagingVM), StatusCodes.Status201Created)]
    public IActionResult CreateImaging([FromBody] DentalImagingVM vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var entity = _mapper.Map<DentalImaging>(vm);
        entity.CreatedBy = GetCurrentUserId();
        var created = dentalService.AddImaging(entity);
        return CreatedAtAction(nameof(GetImagingById), new { id = created.Id }, _mapper.Map<DentalImagingVM>(created));
    }

    [HttpPut("imaging/{id:int}")]
    [ProducesResponseType(typeof(DentalImagingVM), StatusCodes.Status200OK)]
    public IActionResult UpdateImaging(int id, [FromBody] DentalImagingVM vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != vm.Id) return BadRequest("Imaging id mismatch");

        try
        {
            var entity = _mapper.Map<DentalImaging>(vm);
            var updated = dentalService.UpdateImaging(entity, GetCurrentUserId());
            return Ok(_mapper.Map<DentalImagingVM>(updated));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Imaging update forbidden {Id}", id);
            AddModelError(ex.Message);
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(id);
        }
    }

    [HttpDelete("imaging/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteImaging(int id)
    {
        try
        {
            dentalService.DeleteImaging(id, GetCurrentUserId());
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Imaging delete forbidden {Id}", id);
            AddModelError(ex.Message);
            return Forbid();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(id);
        }
    }

    [HttpPost("imaging/upload")]
    [ProducesResponseType(typeof(DentalImagingVM), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UploadImaging([FromForm] DentalImagingUploadVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!IsSupportedImage(vm.File))
            return BadRequest("Only non-empty image files are allowed.");

        if (string.IsNullOrWhiteSpace(vm.Pno) || string.IsNullOrWhiteSpace(vm.ConsultId))
            return BadRequest("pno and consultId are required.");

        var savedPath = SaveUploadedDentalFile(vm.File);

        DentalImaging entity;
        if (vm.Id.HasValue && vm.Id.Value > 0)
        {
            var existing = dentalService.GetImagingById(vm.Id.Value);
            if (existing == null)
                return NotFound(vm.Id.Value);

            existing.Pno = vm.Pno.Trim();
            existing.ConsultId = vm.ConsultId.Trim();
            existing.ImagingDate = vm.ImagingDate ?? DateTime.UtcNow;
            existing.ImagingType = vm.ImagingType;
            existing.ToothRegion = vm.ToothRegion;
            existing.Findings = vm.Findings;
            existing.Impression = vm.Impression;
            existing.Recommendations = vm.Recommendations;
            existing.Notes = vm.Notes;
            existing.FileName = vm.File.FileName;
            existing.FilePath = savedPath;

            entity = dentalService.UpdateImaging(existing, GetCurrentUserId());
        }
        else
        {
            entity = new DentalImaging
            {
                Pno = vm.Pno.Trim(),
                ConsultId = vm.ConsultId.Trim(),
                ImagingDate = vm.ImagingDate ?? DateTime.UtcNow,
                ImagingType = vm.ImagingType,
                ToothRegion = vm.ToothRegion,
                Findings = vm.Findings,
                Impression = vm.Impression,
                Recommendations = vm.Recommendations,
                Notes = vm.Notes,
                FileName = vm.File.FileName,
                FilePath = savedPath,
                CreatedBy = GetCurrentUserId()
            };

            entity = dentalService.AddImaging(entity);
        }

        return CreatedAtAction(nameof(GetImagingById), new { id = entity.Id }, _mapper.Map<DentalImagingVM>(entity));
    }

    private static DateTime ResolveAuditEventDateTime(string? timeZoneId)
    {
        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            return DateTime.UtcNow;
        }

        try
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId.Trim());
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
        }
        catch (TimeZoneNotFoundException)
        {
            return DateTime.UtcNow;
        }
        catch (InvalidTimeZoneException)
        {
            return DateTime.UtcNow;
        }
    }

    private string SaveUploadedDentalFile(IFormFile file)
    {
        var uploadsRoot = EnsureDentalUploadFolder();
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadsRoot, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        file.CopyTo(stream);

        return $"/{DentalUploadFolder.Replace("\\", "/")}/{fileName}";
    }

    private string EnsureDentalUploadFolder()
    {
        var webRoot = environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
        {
            webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var directoryPath = Path.Combine(webRoot, DentalUploadFolder.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(directoryPath);
        return directoryPath;
    }

    private static bool IsSupportedImage(IFormFile file)
    {
        if (file.Length <= 0)
            return false;

        if (string.IsNullOrWhiteSpace(file.ContentType) || !file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
        return allowed.Contains(extension);
    }

    private static string? SerializeTeethStatus(Dictionary<string, ToothStatusVM>? teethStatus)
    {
        if (teethStatus == null || teethStatus.Count == 0)
            return null;

        return JsonSerializer.Serialize(teethStatus, JsonOptions);
    }

    private static Dictionary<string, ToothStatusVM>? DeserializeTeethStatus(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, ToothStatusVM>>(json, JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    private static Dictionary<string, ToothStatusVM>? ResolveTeethStatus(HDentalTreat chart)
    {
        var parsed = DeserializeTeethStatus(chart.TeethStatusJson);
        return parsed ?? BuildTeethStatusFromLegacy(chart);
    }

    private static Dictionary<string, ToothStatusVM>? BuildTeethStatusFromLegacy(HDentalTreat chart)
    {
        var dtype = NormalizeDtypeForStorage(chart.Dtype);
        var key = dtype switch
        {
            "P" => "present",
            "C" => "carious",
            "D" => "decayed",
            "M" => "missing",
            "F" => "filled",
            _ => null
        };

        if (key == null)
            return null;

        var teeth = new[] { "18", "17", "16", "15", "14", "13", "12", "11", "21", "22", "23", "24", "25", "26", "27", "28", "48", "47", "46", "45", "44", "43", "42", "41", "31", "32", "33", "34", "35", "36", "37", "38" };
        var result = new Dictionary<string, ToothStatusVM>();

        foreach (var tooth in teeth)
        {
            var marked = GetLegacyToothFlag(chart, tooth);
            if (marked != true) continue;

            var status = new ToothStatusVM();
            switch (key)
            {
                case "present":
                    status.Present = true;
                    status.Missing = false;
                    break;
                case "carious":
                    status.Carious = true;
                    break;
                case "decayed":
                    status.Decayed = true;
                    break;
                case "missing":
                    status.Missing = true;
                    status.Present = false;
                    break;
                case "filled":
                    status.Filled = true;
                    break;
            }

            result[tooth] = status;
        }

        return result.Count > 0 ? result : null;
    }

    private static bool? GetLegacyToothFlag(HDentalTreat chart, string tooth)
    {
        return tooth switch
        {
            "18" => chart.Aurm3,
            "17" => chart.Aurm2,
            "16" => chart.Aurm1,
            "15" => chart.Aurpm2,
            "14" => chart.Aurpm1,
            "13" => chart.Aurc,
            "12" => chart.Auri2,
            "11" => chart.Auri1,
            "21" => chart.Auli1,
            "22" => chart.Auli2,
            "23" => chart.Aulc,
            "24" => chart.Aulpm1,
            "25" => chart.Aulpm2,
            "26" => chart.Aulm1,
            "27" => chart.Aulm2,
            "28" => chart.Aulm3,
            "48" => chart.Alrm3,
            "47" => chart.Alrm2,
            "46" => chart.Alrm1,
            "45" => chart.Alrpm2,
            "44" => chart.Alrpm1,
            "43" => chart.Alrc,
            "42" => chart.Alri2,
            "41" => chart.Alri1,
            "31" => chart.Alli1,
            "32" => chart.Alli2,
            "33" => chart.Allc,
            "34" => chart.Allpm1,
            "35" => chart.Allpm2,
            "36" => chart.Allm1,
            "37" => chart.Allm2,
            "38" => chart.Allm3,
            _ => null
        };
    }

    private static string? NormalizeDtypeForStorage(string? dtype)
    {
        if (string.IsNullOrWhiteSpace(dtype))
            return null;

        return dtype.Trim().ToUpperInvariant() switch
        {
            "P" or "TEETH PRESENT" => "P",
            "C" or "CARIOUS TEETH" => "C",
            "D" or "DECAYED TEETH" => "D",
            "M" or "MISSING TEETH" => "M",
            "F" or "FILLED TEETH" => "F",
            _ => dtype.Trim().Length > 1 ? dtype.Trim()[..1].ToUpperInvariant() : dtype.Trim().ToUpperInvariant()
        };
    }

    private static string? ExpandDtypeForDisplay(string? dtype)
    {
        if (string.IsNullOrWhiteSpace(dtype))
            return dtype;

        return NormalizeDtypeForStorage(dtype) switch
        {
            "P" => "Teeth Present",
            "C" => "Carious Teeth",
            "D" => "Decayed Teeth",
            "M" => "Missing Teeth",
            "F" => "Filled Teeth",
            _ => dtype
        };
    }

    private static string? SerializeOrthodontics(OrthodonticFormVM? orthodontics)
    {
        if (orthodontics == null)
            return null;

        return JsonSerializer.Serialize(orthodontics, JsonOptions);
    }

    private static OrthodonticFormVM? DeserializeOrthodontics(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            return JsonSerializer.Deserialize<OrthodonticFormVM>(json, JsonOptions);
        }
        catch
        {
            return null;
        }
    }

    private static string? SerializeOralExam(OralExamVM? oralExam)
    {
        if (oralExam == null)
            return null;

        return JsonSerializer.Serialize(oralExam, JsonOptions);
    }

    private static OralExamVM? DeserializeOralExam(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            return JsonSerializer.Deserialize<OralExamVM>(json, JsonOptions);
        }
        catch
        {
            return null;
        }
    }
}
