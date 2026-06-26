using AestheticEMR.Core.Services.Employees.Interfaces;
using AestheticEMR.Server.ViewModels.Employees;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeEntity = AestheticEMR.Core.Models.Employees.Employees;

namespace AestheticEMR.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeeController(
    ILogger<EmployeeController> logger,
    IMapper mapper,
    IEmployeeService employeeService) : BaseApiController(logger, mapper)
{
    [HttpGet("generate-id")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateId()
    {
        var empId = await employeeService.GenerateEmpIdAsync();
        return Ok(empId);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var employees = await employeeService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<EmployeeVM>>(employees));
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> Create([FromBody] EmployeeVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var employee = _mapper.Map<EmployeeEntity>(vm);
        var created = await employeeService.CreateAsync(employee);
        return CreatedAtAction(nameof(GetById), new { id = created.EmpId }, _mapper.Map<EmployeeVM>(created));
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EmployeeVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string id, [FromBody] EmployeeVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!string.Equals(id, vm.EmpId, StringComparison.OrdinalIgnoreCase))
        {
            AddModelError("ID mismatch.");
            return BadRequest(ModelState);
        }

        try
        {
            var employee = _mapper.Map<EmployeeEntity>(vm);
            var updated = await employeeService.UpdateAsync(employee);
            return Ok(_mapper.Map<EmployeeVM>(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
}
