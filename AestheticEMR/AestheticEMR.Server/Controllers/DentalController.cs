using AestheticEMR.Core.Models.Dental;
using AestheticEMR.Core.Models.Legacy;
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
    IDentalService dentalService) : BaseApiController(logger, mapper)
{
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

        return Ok(new DentalEncounterVM
        {
            Chart = _mapper.Map<DentalChartVM>(encounter.Value.Chart),
            Imaging = _mapper.Map<DentalImagingVM>(encounter.Value.Imaging),
            Consulting = _mapper.Map<DentalConsultingVM>(encounter.Value.Consulting)
        });
    }

    [HttpPost("encounter")]
    [ProducesResponseType(typeof(DentalEncounterVM), StatusCodes.Status200OK)]
    public IActionResult SaveEncounter([FromBody] DentalEncounterSaveVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // TreatPlan is hidden from UI and derived from these 3 fields
        vm.Consulting.TreatPlan = null;

        var chart = _mapper.Map<HDentalTreat>(vm.Chart);
        var imaging = _mapper.Map<DentalImaging>(vm.Imaging);
        var consulting = _mapper.Map<HConsulting>(vm.Consulting);

        consulting.ConsultId = chart.ConsultId;
        consulting.PNo = chart.Pno;
        if (string.IsNullOrWhiteSpace(consulting.ClientCat))
            consulting.ClientCat = "PRIVATE";

        try
        {
            var saved = dentalService.SaveEncounter(chart, imaging, consulting, GetCurrentUserId());

            return Ok(new DentalEncounterVM
            {
                Chart = _mapper.Map<DentalChartVM>(saved.Chart),
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
    }

    // ─── Odontogram / Dental Treatment Chart (HDentalTreat) ─────────────────

    [HttpGet("charts")]
    [ProducesResponseType(typeof(IEnumerable<DentalChartVM>), StatusCodes.Status200OK)]
    public IActionResult GetCharts()
    {
        var charts = dentalService.GetCharts();
        return Ok(_mapper.Map<IEnumerable<DentalChartVM>>(charts));
    }

    [HttpGet("charts/{id:long}")]
    [ProducesResponseType(typeof(DentalChartVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetChart(long id)
    {
        var chart = dentalService.GetChartById(id);
        if (chart == null) return NotFound(id);
        return Ok(_mapper.Map<DentalChartVM>(chart));
    }

    [HttpPost("charts")]
    [ProducesResponseType(typeof(DentalChartVM), StatusCodes.Status201Created)]
    public IActionResult CreateChart([FromBody] DentalChartVM vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var entity = _mapper.Map<HDentalTreat>(vm);
        var created = dentalService.AddChart(entity);
        return CreatedAtAction(nameof(GetChart), new { id = created.Id }, _mapper.Map<DentalChartVM>(created));
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
            var updated = dentalService.UpdateChart(entity, GetCurrentUserId());
            return Ok(_mapper.Map<DentalChartVM>(updated));
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
}
