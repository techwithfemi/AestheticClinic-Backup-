using System.Security.Claims;
using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Accounting;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers.Accounting;

[Route("api/accounting/incomes")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewAccountingPolicy)]
public class IncomesController(
    IIncomeService incomeService,
    ILogger<IncomesController> logger,
    IMapper mapper) : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedIncomeResultVM), 200)]
    public async Task<IActionResult> GetPaged([FromQuery] IncomeListQueryVM query, CancellationToken ct)
    {
        var result = await incomeService.GetPagedAsync(_mapper.Map<IncomeListQuery>(query), ct);
        return Ok(_mapper.Map<PagedIncomeResultVM>(result));
    }

    [HttpGet("next-tran-id")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(IncomeTranIdResultVM), 200)]
    public async Task<IActionResult> GetNextTranId(CancellationToken ct)
    {
        var result = await incomeService.GenerateTranIdAsync(ct);
        return Ok(_mapper.Map<IncomeTranIdResultVM>(result));
    }

    [HttpGet("tran-id/{tranId}")]
    [ProducesResponseType(typeof(IEnumerable<IncomeEntryVM>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByTranId(string tranId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(tranId))
        {
            return BadRequest(new { tranId });
        }

        var entries = await incomeService.GetByTranIdAsync(tranId, ct);
        if (!entries.Any())
        {
            return NotFound(new { tranId });
        }

        return Ok(_mapper.Map<IEnumerable<IncomeEntryVM>>(entries));
    }

    [HttpGet("{sNo:long}")]
    [ProducesResponseType(typeof(IncomeEntryVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long sNo, CancellationToken ct)
    {
        var entry = await incomeService.GetByIdAsync(sNo, ct);
        if (entry is null)
        {
            return NotFound(new { sNo });
        }

        return Ok(_mapper.Map<IncomeEntryVM>(entry));
    }

    [HttpGet("income-accounts")]
    [ProducesResponseType(typeof(IEnumerable<IncomeAccountLookupVM>), 200)]
    public async Task<IActionResult> GetIncomeAccounts(CancellationToken ct)
    {
        var accounts = await incomeService.GetIncomeAccountsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<IncomeAccountLookupVM>>(accounts));
    }

    [HttpGet("receiving-accounts")]
    [ProducesResponseType(typeof(IEnumerable<IncomeAccountLookupVM>), 200)]
    public async Task<IActionResult> GetReceivingAccounts(CancellationToken ct)
    {
        var accounts = await incomeService.GetReceivingAccountsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<IncomeAccountLookupVM>>(accounts));
    }

    [HttpGet("transaction-lines/{tranId}")]
    [ProducesResponseType(typeof(IEnumerable<JournalLineVM>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetTransactionLinesByTranId(string tranId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(tranId))
        {
            return BadRequest(new { tranId });
        }

        try
        {
            var lines = await incomeService.GetTransactionLinesByTranIdAsync(tranId, ct);
            if (!lines.Any())
            {
                return NotFound(new { tranId });
            }

            return Ok(_mapper.Map<IEnumerable<JournalLineVM>>(lines));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving transaction lines for TranId {TranId}", tranId);
            AddModelError("Unable to retrieve transaction lines");
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPost("batch")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(IncomeBatchSaveResultVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateBatch([FromBody] IncomeBatchSaveVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var created = await incomeService.CreateBatchAsync(_mapper.Map<IncomeBatchSaveRequest>(model), GetCurrentUserName(), ct);
            return StatusCode(201, _mapper.Map<IncomeBatchSaveResultVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error creating batch income entries");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPut("tran-id/{tranId}")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(IncomeBatchSaveResultVM), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateByTranId(string tranId, [FromBody] IncomeBatchSaveVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var updated = await incomeService.UpdateByTranIdAsync(tranId, _mapper.Map<IncomeBatchSaveRequest>(model), GetCurrentUserName(), ct);
            return Ok(_mapper.Map<IncomeBatchSaveResultVM>(updated));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error updating income transaction {TranId}", tranId);
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpDelete("tran-id/{tranId}")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteByTranId(string tranId, [FromQuery] string period, [FromQuery] string coyID, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(tranId) || string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(coyID))
        {
            return BadRequest(new { tranId, period, coyID });
        }

        try
        {
            await incomeService.DeleteByTranIdAsync(tranId, GetCurrentUserName(), period, coyID, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error deleting income transaction {TranId}", tranId);
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
