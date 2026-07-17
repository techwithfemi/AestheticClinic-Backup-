using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.Configuration;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AppointmentController(
    ILogger<AppointmentController> logger,
    IMapper mapper,
    IAppointmentService appointmentService,
    IOptions<AppSettings> appSettings)
    : BaseApiController(logger, mapper)
{
    private readonly bool _enableAppointmentSms = appSettings.Value.AppointmentNotificationConfig?.EnableSms ?? true;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentVM>), 200)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var appointments = await appointmentService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<AppointmentVM>>(appointments));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments");
            AddModelError("Unable to retrieve appointments");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AppointmentVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var appointment = await appointmentService.GetByIdAsync(id);
            if (appointment is null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<AppointmentVM>(appointment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment {Id}", id);
            AddModelError("Unable to retrieve appointment");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("clinic-types")]
    [ProducesResponseType(typeof(IEnumerable<string>), 200)]
    public async Task<IActionResult> GetClinicTypes()
    {
        try
        {
            var clinicTypes = await appointmentService.GetClinicTypesAsync();
            return Ok(clinicTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving clinic types");
            AddModelError("Unable to retrieve clinic types");
            return BadRequest(ModelState);
        }
    }
    [HttpGet("employees")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeLookupVM>), 200)]
    public async Task<IActionResult> GetEmployees()
    {
        try
        {
            var employees = await appointmentService.GetEmployeesAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeLookupVM>>(employees));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            AddModelError("Unable to retrieve employees");
            return BadRequest(ModelState);
        }
    }


    [HttpPost]
    [ProducesResponseType(typeof(AppointmentVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] AppointmentVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var appointment = _mapper.Map<hAppointment>(model);
            appointment.ID = 0;
            var sendSms = _enableAppointmentSms && (model.SendSms ?? true);
            var created = await appointmentService.CreateAsync(appointment, sendSms);
            return CreatedAtAction(nameof(GetById), new { id = created.ID }, _mapper.Map<AppointmentVM>(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment");
            AddModelError(ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(AppointmentVM), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(long id, [FromBody] AppointmentVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await appointmentService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            _mapper.Map(model, existing);
            existing.ID = id;

            var sendSms = _enableAppointmentSms && (model.SendSms ?? true);
            var updated = await appointmentService.UpdateAsync(existing, sendSms);
            return Ok(_mapper.Map<AppointmentVM>(updated));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating appointment {Id}", id);
            AddModelError("Unable to update appointment");
            return BadRequest(ModelState);
        }
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var existing = await appointmentService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            await appointmentService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting appointment {Id}", id);
            AddModelError("Unable to delete appointment");
            return BadRequest(ModelState);
        }
    }
}

