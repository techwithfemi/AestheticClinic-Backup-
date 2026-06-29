using AestheticEMR.Core.Services.Employees.Interfaces;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Employees;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeEntity = AestheticEMR.Core.Models.Employees.Employees;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewEmployeesPolicy)]
public class EmployeeController(
    ILogger<EmployeeController> logger,
    IMapper mapper,
    IEmployeeService employeeService) : BaseApiController(logger, mapper)
{
    [HttpGet("generate-id")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    public async Task<IActionResult> GenerateId()
    {
        var empId = await employeeService.GenerateEmpIdAsync();
        return Ok(empId);
    }

    [HttpGet("designations")]
    [ProducesResponseType(typeof(IEnumerable<DesignationVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDesignations()
    {
        var designations = await employeeService.GetDesignationsAsync();
        return Ok(_mapper.Map<IEnumerable<DesignationVM>>(designations));
    }

    [HttpGet("departments")]
    [ProducesResponseType(typeof(IEnumerable<EmpDepartmentVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await employeeService.GetDepartmentsAsync();
        return Ok(_mapper.Map<IEnumerable<EmpDepartmentVM>>(departments));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var employees = await employeeService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<EmployeeVM>>(employees));
    }

    [HttpGet("{**id}")]
    [ProducesResponseType(typeof(EmployeeVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(string id)
    {
        var employee = await employeeService.GetByIdAsync(id);
        if (employee == null)
            return NotFound();
        return Ok(_mapper.Map<EmployeeVM>(employee));
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmployeeVM), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    public async Task<IActionResult> Create([FromBody] EmployeeVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var employee = _mapper.Map<EmployeeEntity>(vm);
        var created = await employeeService.CreateAsync(employee);
        return CreatedAtAction(nameof(GetById), new { id = created.EmpId }, _mapper.Map<EmployeeVM>(created));
    }

    [HttpPut("{**id}")]
    [ProducesResponseType(typeof(EmployeeVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    public async Task<IActionResult> Update(string id, [FromBody] EmployeeVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Defense in depth: route param is the source of truth for the PK.
        // Legacy IDs (e.g. "HR/001") and new IDs (e.g. "HR-0000001") both work.
        if (string.IsNullOrWhiteSpace(id))
        {
            AddModelError("Missing employee id in route.");
            return BadRequest(ModelState);
        }
        vm.EmpId = id;

        try
        {
            var employee = _mapper.Map<EmployeeEntity>(vm);
            employee.EmpId = id; // ensure PK is never lost through mapping
            var updated = await employeeService.UpdateAsync(employee);
            return Ok(_mapper.Map<EmployeeVM>(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update employee {EmpId}", id);
            return Problem(
                title: "Update failed",
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpDelete("{**id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await employeeService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
