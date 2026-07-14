using System.Security.Claims;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Models;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/roster/groups")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewEmployeesPolicy)]
public class RosterGroupController(
    ILogger<RosterGroupController> logger,
    IMapper mapper,
    IRosterGroupService rosterGroupService) : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RosterGroupGridItemVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var rows = await rosterGroupService.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<RosterGroupGridItemVM>>(rows));
    }

    [HttpGet("departments")]
    [ProducesResponseType(typeof(IEnumerable<RosterGroupDepartmentItemVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDepartments(CancellationToken ct)
    {
        var rows = await rosterGroupService.GetDepartmentsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<RosterGroupDepartmentItemVM>>(rows));
    }

    [HttpGet("staff")]
    [ProducesResponseType(typeof(IEnumerable<RosterGroupAvailableStaffItemVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableStaff([FromQuery] string? deptId, CancellationToken ct)
    {
        var rows = await rosterGroupService.GetAvailableStaffAsync(deptId, ct);
        return Ok(_mapper.Map<IEnumerable<RosterGroupAvailableStaffItemVM>>(rows));
    }

    [HttpGet("current-department")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentDepartmentName(CancellationToken ct)
    {
        var deptName = await rosterGroupService.GetCurrentDepartmentNameAsync(ct);
        return Ok(deptName);
    }

    [HttpGet("{rosterGrpId:long}")]
    [ProducesResponseType(typeof(RosterGroupItemVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long rosterGrpId, CancellationToken ct)
    {
        var row = await rosterGroupService.GetByIdAsync(rosterGrpId, ct);
        return row is null ? NotFound(new { rosterGrpId }) : Ok(_mapper.Map<RosterGroupItemVM>(row));
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(typeof(RosterGroupItemVM), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] RosterGroupSaveVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var created = await rosterGroupService.CreateAsync(_mapper.Map<RosterGroupSaveRequest>(model), GetCurrentUserName(), ct);
            return StatusCode(201, _mapper.Map<RosterGroupItemVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error creating roster group");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPut("{rosterGrpId:long}")]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(typeof(RosterGroupItemVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long rosterGrpId, [FromBody] RosterGroupSaveVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var updated = await rosterGroupService.UpdateAsync(rosterGrpId, _mapper.Map<RosterGroupSaveRequest>(model), GetCurrentUserName(), ct);
            return Ok(_mapper.Map<RosterGroupItemVM>(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { rosterGrpId });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error updating roster group {RosterGrpId}", rosterGrpId);
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpDelete("{rosterGrpId:long}")]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long rosterGrpId, CancellationToken ct)
    {
        var removed = await rosterGroupService.DeleteAsync(rosterGrpId, GetCurrentUserName(), ct);
        return removed ? NoContent() : NotFound(new { rosterGrpId });
    }

    private string GetCurrentUserName()
    {
        return User.Identity?.Name
               ?? User.FindFirstValue("preferred_username")
               ?? User.FindFirstValue(ClaimTypes.Name)
               ?? GetCurrentUserId();
    }
}
