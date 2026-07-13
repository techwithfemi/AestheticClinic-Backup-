using System.Security.Claims;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Models;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/roster/shift-master")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewEmployeesPolicy)]
public class ShiftMasterController(
    ILogger<ShiftMasterController> logger,
    IMapper mapper,
    IShiftMasterService shiftMasterService) : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ShiftMasterItemVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var rows = await shiftMasterService.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<ShiftMasterItemVM>>(rows));
    }

    [HttpGet("departments")]
    [ProducesResponseType(typeof(IEnumerable<DepartmentLookupVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDepartments(CancellationToken ct)
    {
        var rows = await shiftMasterService.GetDepartmentsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<DepartmentLookupVM>>(rows));
    }

    [HttpGet("{shiftId:long}")]
    [ProducesResponseType(typeof(ShiftMasterDetailVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long shiftId, CancellationToken ct)
    {
        var row = await shiftMasterService.GetByIdAsync(shiftId, ct);
        return row is null ? NotFound(new { shiftId }) : Ok(_mapper.Map<ShiftMasterDetailVM>(row));
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(typeof(ShiftMasterDetailVM), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] ShiftMasterDetailVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var created = await shiftMasterService.CreateAsync(_mapper.Map<ShiftMasterSaveRequest>(model), GetCurrentUserName(), ct);
            return StatusCode(201, _mapper.Map<ShiftMasterDetailVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error creating shift master");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPut("{shiftId:long}")]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(typeof(ShiftMasterDetailVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long shiftId, [FromBody] ShiftMasterDetailVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var updated = await shiftMasterService.UpdateAsync(shiftId, _mapper.Map<ShiftMasterSaveRequest>(model), GetCurrentUserName(), ct);
            return Ok(_mapper.Map<ShiftMasterDetailVM>(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { shiftId });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error updating shift master {ShiftId}", shiftId);
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpDelete("{shiftId:long}")]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long shiftId, CancellationToken ct)
    {
        try
        {
            var removed = await shiftMasterService.DeleteAsync(shiftId, GetCurrentUserName(), ct);
            return removed ? NoContent() : NotFound(new { shiftId });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error deleting shift master {ShiftId}", shiftId);
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    private string GetCurrentUserName()
    {
        return User.Identity?.Name
               ?? User.FindFirstValue("preferred_username")
               ?? User.FindFirstValue(ClaimTypes.Name)
               ?? GetCurrentUserId();
    }
}
