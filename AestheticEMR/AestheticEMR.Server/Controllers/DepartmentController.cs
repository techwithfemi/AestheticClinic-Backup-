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
public class DepartmentController(
    ILogger<DepartmentController> logger,
    IMapper mapper,
    IDepartmentService departmentService) : BaseApiController(logger, mapper)
{
    [HttpGet("generate-id")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateId()
    {
        try
        {
            var id = await departmentService.GenerateDepartmentIdAsync();
            return Ok(id);
        }
        catch (InvalidOperationException ex)
        {
            // ID limit reached — surface as 400 so the UI can show a meaningful message.
            logger.LogWarning(ex, "Failed to generate department id");
            return Problem(
                title: "Cannot generate department id",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DepartmentVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var departments = await departmentService.GetAllAsync();

        // Hydrate InUseCount from a single grouped query so the UI can warn before delete.
        // Done here (not in the service) to keep the service contract lean.
        var inUseMap = await departmentService.GetInUseCountsAsync();

        var vms = departments.Select(d =>
        {
            var vm = _mapper.Map<DepartmentVM>(d);
            vm.InUseCount = inUseMap.TryGetValue(d.DeptId ?? string.Empty, out var n) ? n : 0;
            return vm;
        });

        return Ok(vms);
    }

    [HttpGet("{**id}")]
    [ProducesResponseType(typeof(DepartmentVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var department = await departmentService.GetByIdAsync(id);
        if (department == null)
            return NotFound();
        return Ok(_mapper.Map<DepartmentVM>(department));
    }

    [HttpPost]
    [ProducesResponseType(typeof(DepartmentVM), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] DepartmentVM vm)
    {
        if (!ModelState.IsValid)
        {
            logger.LogWarning("Create department rejected by ModelState: {Errors}",
                string.Join("; ", ModelState.SelectMany(kv => kv.Value!.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        try
        {
            // The server is the source of truth for the id — ignore any client value
            // and let the service generate it inside its transaction.
            var entity = _mapper.Map<EmpDepartments>(vm);
            entity.DeptId = string.Empty; // sentinel — service will overwrite

            logger.LogInformation("Creating department with name '{Name}' (client-supplied id='{ClientId}')",
                vm.DeptName, vm.DeptId ?? "<none>");

            var created = await departmentService.CreateAsync(entity);
            logger.LogInformation("Created department {DeptId}", created.DeptId);
            return CreatedAtAction(nameof(GetById), new { id = created.DeptId }, _mapper.Map<DepartmentVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Failed to create department (validation)");
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
            logger.LogError(ex, "Database error creating department '{Name}'", vm.DeptName);
            return Problem(
                title: "Create failed",
                detail: ex.InnerException?.Message ?? ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating department '{Name}'", vm.DeptName);
            return Problem(
                title: "Create failed",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{**id}")]
    [ProducesResponseType(typeof(DepartmentVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] DepartmentVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(id))
        {
            AddModelError("Missing department id in route.");
            return BadRequest(ModelState);
        }

        // Route is the source of truth for the PK.
        vm.DeptId = id;

        try
        {
            var entity = _mapper.Map<EmpDepartments>(vm);
            entity.DeptId = id;
            var updated = await departmentService.UpdateAsync(entity);
            return Ok(_mapper.Map<DepartmentVM>(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database error updating department {DeptId}", id);
            return Problem(
                title: "Update failed",
                detail: ex.InnerException?.Message ?? ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update department {DeptId}", id);
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
            var removed = await departmentService.DeleteAsync(id);
            return removed ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            // Department is referenced by employees → 409 Conflict.
            logger.LogWarning(ex, "Refused to delete department {DeptId} because it is in use", id);
            return Problem(
                title: "Department in use",
                detail: ex.Message,
                statusCode: StatusCodes.Status409Conflict);
        }
    }
}
