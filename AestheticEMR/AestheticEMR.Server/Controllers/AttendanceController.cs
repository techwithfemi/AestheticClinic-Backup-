using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AttendanceController(ILogger<AttendanceController> logger, IMapper mapper, IAttendanceService attendanceService)
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
            return Ok(_mapper.Map<AttendanceVM>(updated));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating attendance record {Id}", id);
            AddModelError("Unable to update attendance record");
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
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting attendance record {Id}", id);
            AddModelError("Unable to delete attendance record");
            return BadRequest(ModelState);
        }
    }
}
