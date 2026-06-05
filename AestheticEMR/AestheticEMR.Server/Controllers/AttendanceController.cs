using AestheticEMR.Core.Models.Aesthetic;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Aesthetics;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.Services;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AttendanceController(
    ILogger<AttendanceController> logger,
    IMapper mapper,
    IAttendanceService attendanceService,
    IAuditService auditService)
    : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AttendanceVM>), 200)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var records = await attendanceService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<AttendanceVM>>(records));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving attendance records");
            AddModelError("Unable to retrieve attendance records");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AttendanceVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var record = await attendanceService.GetByIdAsync(id);
            if (record is null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<AttendanceVM>(record));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving attendance record {Id}", id);
            AddModelError("Unable to retrieve attendance record");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("{id}/consulting-notes")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetConsultingNotes(string id)
    {
        try
        {
            var notes = await attendanceService.GetConsultingNotesAsync(id);
            if (notes is null)
                return NoContent();

            return Ok(notes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving consulting notes for {Id}", id);
            AddModelError("Unable to retrieve consulting notes");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("clinic-types")]
    [ProducesResponseType(typeof(IEnumerable<string>), 200)]
    public async Task<IActionResult> GetClinicTypes()
    {
        try
        {
            var clinicTypes = await attendanceService.GetClinicTypesAsync();
            return Ok(clinicTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving clinic types");
            AddModelError("Unable to retrieve clinic types");
            return BadRequest(ModelState);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(AttendanceVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] AttendanceVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var record = _mapper.Map<HRecord>(model);
            record.ConsultId = string.Empty;
            var created = await attendanceService.CreateAsync(record);

            await LogAttendanceAuditAsync(
                "Create",
                "Attendance recorded",
                $"Attendance created for patient {created.PNo} ({created.ClinicType}).",
                created);

            return CreatedAtAction(nameof(GetById), new { id = created.ConsultId }, _mapper.Map<AttendanceVM>(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating attendance record");
            AddModelError(ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AttendanceVM), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(string id, [FromBody] AttendanceVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await attendanceService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            _mapper.Map(model, existing);
            existing.ConsultId = id;

            var updated = await attendanceService.UpdateAsync(existing);

            await LogAttendanceAuditAsync(
                "Update",
                "Attendance updated",
                $"Attendance updated for patient {updated.PNo} ({updated.ClinicType}).",
                updated);

            return Ok(_mapper.Map<AttendanceVM>(updated));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating attendance record {Id}", id);
            AddModelError(ex.GetBaseException().Message);
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var existing = await attendanceService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            await attendanceService.DeleteAsync(id);

            await LogAttendanceAuditAsync(
                "Delete",
                "Attendance deleted",
                $"Attendance deleted for patient {existing.PNo} ({existing.ClinicType}).",
                existing);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Delete blocked for attendance record {Id}", id);
            AddModelError(ex.Message);
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting attendance record {Id}", id);
            AddModelError("Unable to delete attendance record");
            return BadRequest(ModelState);
        }
    }

    private async Task LogAttendanceAuditAsync(string eventType, string summary, string details, HRecord record)
    {
        var performedBy = Utilities.GetUserId(User) ?? User?.Identity?.Name;
        var sourceIp = HttpContext.Connection.RemoteIpAddress?.ToString();

        await auditService.LogEventAsync(new AuditLog
        {
            TranCode = string.IsNullOrWhiteSpace(record.ConsultId)
                ? (string.IsNullOrWhiteSpace(record.PNo) ? "GENERAL" : record.PNo)
                : record.ConsultId,
            EventType = eventType,
            Summary = summary,
            Details = details,
            Severity = "Info",
            EntityType = nameof(HRecord),
            EntityId = record.RecId,
            UserId = performedBy,
            PerformedBy = performedBy,
            SourceIp = sourceIp,
            Tags = "#attendance #frontdesk",
            Status = "Logged"
        });
    }
}
