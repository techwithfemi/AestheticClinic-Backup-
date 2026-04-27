using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[Authorize]
public class HPatientController(ILogger<HPatientController> logger, IMapper mapper, IHPatientService patientService)
    : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HPatientVM>), 200)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var patients = await patientService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<HPatientVM>>(patients));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patients");
            AddModelError("Unable to retrieve patients");
            return BadRequest(ModelState);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HPatientVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var patient = await patientService.GetByIdAsync(id);
            if (patient is null)
            {
                return NotFound(id);
            }

            return Ok(_mapper.Map<HPatientVM>(patient));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient {Id}", id);
            AddModelError("Unable to retrieve patient");
            return BadRequest(ModelState);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(HPatientVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] HPatientVM model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var patient = _mapper.Map<HPatient>(model);
            // Pno is auto-generated inside CreateAsync via the getIDNo stored procedure
            patient.Pno = string.Empty; // will be replaced in service
            var created = await patientService.CreateAsync(patient);
            return CreatedAtAction(nameof(GetById), new { id = created.Pno }, _mapper.Map<HPatientVM>(created));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            AddModelError(ex.Message);
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(HPatientVM), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(string id, [FromBody] HPatientVM model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var existing = await patientService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            _mapper.Map(model, existing);
            existing.Pno = id;

            var updated = await patientService.UpdateAsync(existing);
            return Ok(_mapper.Map<HPatientVM>(updated));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating patient {Id}", id);
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
            var existing = await patientService.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound(id);
            }

            await patientService.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Delete blocked for patient {Id}", id);
            AddModelError(ex.Message);
            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting patient {Id}", id);
            AddModelError("Unable to delete patient");
            return BadRequest(ModelState);
        }
    }
}
