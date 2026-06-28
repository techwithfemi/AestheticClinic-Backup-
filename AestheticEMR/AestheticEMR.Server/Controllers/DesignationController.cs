using AestheticEMR.Core.Models.Employees;
using AestheticEMR.Core.Services.Employees.Interfaces;
using AestheticEMR.Server.ViewModels.Employees;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DesignationController(
    ILogger<DesignationController> logger,
    IMapper mapper,
    IDesignationService designationService) : BaseApiController(logger, mapper)
{
    [HttpGet("generate-id")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateId()
    {
        try
        {
            var id = await designationService.GenerateDesignationIdAsync();
            return Ok(id);
        }
        catch (InvalidOperationException ex)
        {
            // ID limit reached — surface as 400 so the UI can show a meaningful message.
            logger.LogWarning(ex, "Failed to generate designation id");
            return Problem(
                title: "Cannot generate designation id",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DesignationVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var designations = await designationService.GetAllAsync();

        // Hydrate InUseCount from a single grouped query so the UI can warn before delete.
        // Done here (not in the service) to keep the service contract lean.
        var inUseMap = await designationService.GetInUseCountsAsync();

        var vms = designations.Select(d =>
        {
            var vm = _mapper.Map<DesignationVM>(d);
            vm.InUseCount = inUseMap.TryGetValue(d.desID ?? string.Empty, out var n) ? n : 0;
            return vm;
        });

        return Ok(vms);
    }

    [HttpGet("{**id}")]
    [ProducesResponseType(typeof(DesignationVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var designation = await designationService.GetByIdAsync(id);
        if (designation == null)
            return NotFound();
        return Ok(_mapper.Map<DesignationVM>(designation));
    }

    [HttpPost]
    [ProducesResponseType(typeof(DesignationVM), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DesignationVM vm)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Create designation rejected by ModelState: {Errors}",
                string.Join("; ", ModelState.SelectMany(kv => kv.Value!.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        try
        {
            // The server is the source of truth for the id — ignore any client value
            // and let the service generate it inside its transaction.
            var entity = _mapper.Map<Designation>(vm);
            entity.desID = string.Empty; // sentinel — service will overwrite

            logger.LogInformation("Creating designation with name '{Name}' (client-supplied id='{ClientId}')",
                vm.DesignationName, vm.DesignationId ?? "<none>");

            var created = await designationService.CreateAsync(entity);
            logger.LogInformation("Created designation {DesId}", created.desID);
            return CreatedAtAction(nameof(GetById), new { id = created.desID }, _mapper.Map<DesignationVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Failed to create designation (validation)");
            return Problem(
                title: "Create failed",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (DbUpdateException ex)
        {
            // SQL-level failures (unique constraint, FK violation, etc.) surface here.
            // Roll back the transaction and return a 400 with the real message so the
            // UI can show it instead of a generic 500.
            logger.LogError(ex, "Database error creating designation '{Name}'", vm.DesignationName);
            return Problem(
                title: "Create failed",
                detail: ex.InnerException?.Message ?? ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating designation '{Name}'", vm.DesignationName);
            return Problem(
                title: "Create failed",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{**id}")]
    [ProducesResponseType(typeof(DesignationVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] DesignationVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(id))
        {
            AddModelError("Missing designation id in route.");
            return BadRequest(ModelState);
        }

        // Route is the source of truth for the PK.
        vm.DesignationId = id;

        try
        {
            var entity = _mapper.Map<Designation>(vm);
            entity.desID = id;
            var updated = await designationService.UpdateAsync(entity);
            return Ok(_mapper.Map<DesignationVM>(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error updating designation {DesId}", id);
            return Problem(
                title: "Update failed",
                detail: ex.InnerException?.Message ?? ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update designation {DesId}", id);
            return Problem(
                title: "Update failed",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpDelete("{**id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var removed = await designationService.DeleteAsync(id);
            return removed ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            // Designation is referenced by employees → 409 Conflict.
            logger.LogWarning(ex, "Refused to delete designation {DesId} because it is in use", id);
            return Problem(
                title: "Designation in use",
                detail: ex.Message,
                statusCode: StatusCodes.Status409Conflict);
        }
    }
}