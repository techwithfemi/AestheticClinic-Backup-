using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Accounting;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers.Accounting;

[Route("api/accounting/chart-of-accounts")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewAccountingPolicy)]
public class ChartOfAccountsController(
    IChartOfAccountService chartOfAccountService,
    ILogger<ChartOfAccountsController> logger,
    IMapper mapper) : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedChartOfAccountResultVM), 200)]
    public async Task<IActionResult> GetPaged([FromQuery] ChartOfAccountListQueryVM query, CancellationToken ct)
    {
        var result = await chartOfAccountService.GetPagedAsync(_mapper.Map<ChartOfAccountListQuery>(query), ct);
        return Ok(_mapper.Map<PagedChartOfAccountResultVM>(result));
    }

    [HttpGet("{sNo:long}")]
    [ProducesResponseType(typeof(ChartOfAccountEntryVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long sNo, CancellationToken ct)
    {
        var entry = await chartOfAccountService.GetByIdAsync(sNo, ct);
        if (entry is null)
        {
            return NotFound(new { sNo });
        }

        return Ok(_mapper.Map<ChartOfAccountEntryVM>(entry));
    }

    [HttpGet("defaults")]
    [ProducesResponseType(typeof(ChartOfAccountDefaultsVM), 200)]
    public async Task<IActionResult> GetDefaults(CancellationToken ct)
    {
        var defaults = await chartOfAccountService.GetDefaultsAsync(ct);
        return Ok(_mapper.Map<ChartOfAccountDefaultsVM>(defaults));
    }

    [HttpGet("groups")]
    [ProducesResponseType(typeof(IEnumerable<ChartOfAccountGroupLookupVM>), 200)]
    public async Task<IActionResult> GetGroups(CancellationToken ct)
    {
        var groups = await chartOfAccountService.GetGroupsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<ChartOfAccountGroupLookupVM>>(groups));
    }

    [HttpGet("next-account-no")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetNextAccountNo([FromQuery] string groupId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(groupId))
        {
            AddModelError("Group ID is required.");
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        var accountNo = await chartOfAccountService.GetNextAccountNoAsync(groupId, ct);
        return Ok(new { accountNo });
    }

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(ChartOfAccountEntryVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] ChartOfAccountEntryVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var created = await chartOfAccountService.CreateAsync(_mapper.Map<ChartOfAccountEntry>(model), ct);
            if (created is null)
            {
                AddModelError("Unable to read created account record.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            return CreatedAtAction(nameof(GetById), new { sNo = created.SNo }, _mapper.Map<ChartOfAccountEntryVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error creating chart of account");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPut("{sNo:long}")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(ChartOfAccountEntryVM), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(long sNo, [FromBody] ChartOfAccountEntryVM model, CancellationToken ct)
    {
        if (model.SNo is not null && model.SNo.Value != sNo)
        {
            AddModelError("SNo in URL does not match body");
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        var existing = await chartOfAccountService.GetByIdAsync(sNo, ct);
        if (existing is null)
        {
            return NotFound(new { sNo });
        }

        try
        {
            var entry = _mapper.Map<ChartOfAccountEntry>(model);
            entry.SNo = sNo;
            var updated = await chartOfAccountService.UpdateAsync(entry, ct);
            if (updated is null)
            {
                AddModelError("Unable to read updated account record.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            return Ok(_mapper.Map<ChartOfAccountEntryVM>(updated));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error updating chart of account {SNo}", sNo);
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpDelete("{sNo:long}")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(long sNo, CancellationToken ct)
    {
        var existing = await chartOfAccountService.GetByIdAsync(sNo, ct);
        if (existing is null)
        {
            return NotFound(new { sNo });
        }

        try
        {
            await chartOfAccountService.DeleteAsync(sNo, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error deleting chart of account {SNo}", sNo);
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }
}
