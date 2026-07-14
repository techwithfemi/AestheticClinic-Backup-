using System.Security.Claims;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Models;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/roster")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewEmployeesPolicy)]
public class RosterController(
    ILogger<RosterController> logger,
    IMapper mapper,
    IRosterService rosterService) : BaseApiController(logger, mapper)
{
    [HttpGet("lookups")]
    [ProducesResponseType(typeof(RosterLookupsVM), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLookups(CancellationToken ct)
    {
        var lookups = await rosterService.GetLookupsAsync(ct);
        return Ok(_mapper.Map<RosterLookupsVM>(lookups));
    }

    [HttpGet("grid")]
    [ProducesResponseType(typeof(IEnumerable<RosterGridItemVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGrid([FromQuery] RosterGridQueryVM query, CancellationToken ct)
    {
        var rows = await rosterService.GetGridAsync(_mapper.Map<RosterGridQuery>(query), ct);
        return Ok(_mapper.Map<IEnumerable<RosterGridItemVM>>(rows));
    }

    [HttpGet("existing")]
    [ProducesResponseType(typeof(IEnumerable<RosterGridItemVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExisting([FromQuery] RosterEditorQueryVM query, CancellationToken ct)
    {
        var rows = await rosterService.GetExistingAsync(_mapper.Map<RosterEditorQuery>(query), ct);
        return Ok(_mapper.Map<IEnumerable<RosterGridItemVM>>(rows));
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(typeof(RosterSaveResultVM), StatusCodes.Status201Created)]
    public async Task<IActionResult> Save([FromBody] RosterSaveVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var result = await rosterService.SaveAsync(_mapper.Map<RosterSaveRequest>(model), GetCurrentUserName(), ct);
            return StatusCode(201, _mapper.Map<RosterSaveResultVM>(result));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Roster save validation error");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpDelete("{sNo:long}")]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long sNo, CancellationToken ct)
    {
        await rosterService.DeleteAsync(new RosterDeleteRequest { SNo = sNo }, GetCurrentUserName(), ct);
        return NoContent();
    }

    private string GetCurrentUserName()
    {
        return User.Identity?.Name
               ?? User.FindFirstValue("preferred_username")
               ?? User.FindFirstValue(ClaimTypes.Name)
               ?? GetCurrentUserId();
    }
}
