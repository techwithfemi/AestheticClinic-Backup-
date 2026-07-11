using System.Security.Claims;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Models;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Legacy;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers;

[Route("api/roster/shift-details")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewEmployeesPolicy)]
public class ShiftDetailsController(
    ILogger<ShiftDetailsController> logger,
    IMapper mapper,
    IShiftDetailService shiftDetailService) : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ShiftDetailVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var rows = await shiftDetailService.GetAllAsync(ct);
        return Ok(_mapper.Map<IEnumerable<ShiftDetailVM>>(rows));
    }

    [HttpGet("lookups")]
    [ProducesResponseType(typeof(IEnumerable<ShiftLookupVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLookups(CancellationToken ct)
    {
        var rows = await shiftDetailService.GetShiftLookupsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<ShiftLookupVM>>(rows));
    }

    [HttpGet("{shiftId:long}")]
    [ProducesResponseType(typeof(ShiftDetailVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long shiftId, CancellationToken ct)
    {
        var row = await shiftDetailService.GetByIdAsync(shiftId, ct);
        return row is null ? NotFound(new { shiftId }) : Ok(_mapper.Map<ShiftDetailVM>(row));
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(typeof(ShiftDetailVM), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] ShiftDetailVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var created = await shiftDetailService.CreateAsync(_mapper.Map<ShiftDetailItem>(model), GetCurrentUserName(), ct);
            return StatusCode(201, _mapper.Map<ShiftDetailVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error creating shift detail");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPut("{shiftId:long}")]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(typeof(ShiftDetailVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long shiftId, [FromBody] ShiftDetailVM model, CancellationToken ct)
    {
        if (shiftId <= 0)
        {
            AddModelError("Shift id is required.");
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            model.ShiftId = shiftId;
            var updated = await shiftDetailService.UpdateAsync(_mapper.Map<ShiftDetailItem>(model), GetCurrentUserName(), ct);
            return Ok(_mapper.Map<ShiftDetailVM>(updated));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { shiftId });
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error updating shift detail {ShiftId}", shiftId);
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpDelete("{shiftId:long}")]
    [Authorize(Policy = AuthPolicies.ManageEmployeesPolicy)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long shiftId, CancellationToken ct)
    {
        var removed = await shiftDetailService.DeleteAsync(shiftId, GetCurrentUserName(), ct);
        return removed ? NoContent() : NotFound(new { shiftId });
    }

    private string GetCurrentUserName()
    {
        return User.Identity?.Name
               ?? User.FindFirstValue("preferred_username")
               ?? User.FindFirstValue(ClaimTypes.Name)
               ?? GetCurrentUserId();
    }
}
