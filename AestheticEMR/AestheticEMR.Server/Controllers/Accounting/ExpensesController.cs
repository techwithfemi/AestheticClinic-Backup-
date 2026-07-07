using System.Security.Claims;
using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using AestheticEMR.Server.Authorization;
using AestheticEMR.Server.ViewModels.Accounting;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AestheticEMR.Server.Controllers.Accounting;

[Route("api/accounting/expenses")]
[ApiController]
[Authorize(Policy = AuthPolicies.ViewAccountingPolicy)]
public class ExpensesController(
    IExpenseService expenseService,
    ILogger<ExpensesController> logger,
    IMapper mapper) : BaseApiController(logger, mapper)
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedExpenseResultVM), 200)]
    public async Task<IActionResult> GetPaged([FromQuery] ExpenseListQueryVM query, CancellationToken ct)
    {
        var result = await expenseService.GetPagedAsync(_mapper.Map<ExpenseListQuery>(query), ct);
        return Ok(_mapper.Map<PagedExpenseResultVM>(result));
    }

    [HttpGet("next-tran-id")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(ExpenseTranIdResultVM), 200)]
    public async Task<IActionResult> GetNextTranId(CancellationToken ct)
    {
        var result = await expenseService.GenerateTranIdAsync(ct);
        return Ok(_mapper.Map<ExpenseTranIdResultVM>(result));
    }

    [HttpGet("tran-id/{tranId}")]
    [ProducesResponseType(typeof(IEnumerable<ExpenseEntryVM>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByTranId(string tranId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(tranId))
        {
            return BadRequest(new { tranId });
        }

        var entries = await expenseService.GetByTranIdAsync(tranId, ct);
        if (!entries.Any())
        {
            return NotFound(new { tranId });
        }

        return Ok(_mapper.Map<IEnumerable<ExpenseEntryVM>>(entries));
    }

    [HttpGet("{sNo:long}")]
    [ProducesResponseType(typeof(ExpenseEntryVM), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(long sNo, CancellationToken ct)
    {
        var entry = await expenseService.GetByIdAsync(sNo, ct);
        if (entry is null)
        {
            return NotFound(new { sNo });
        }

        return Ok(_mapper.Map<ExpenseEntryVM>(entry));
    }

    [HttpGet("expense-accounts")]
    [ProducesResponseType(typeof(IEnumerable<ExpenseAccountLookupVM>), 200)]
    public async Task<IActionResult> GetExpenseAccounts(CancellationToken ct)
    {
        var accounts = await expenseService.GetExpenseAccountsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<ExpenseAccountLookupVM>>(accounts));
    }

    [HttpGet("paying-accounts")]
    [ProducesResponseType(typeof(IEnumerable<ExpenseAccountLookupVM>), 200)]
    public async Task<IActionResult> GetPayingAccounts(CancellationToken ct)
    {
        var accounts = await expenseService.GetPayingAccountsAsync(ct);
        return Ok(_mapper.Map<IEnumerable<ExpenseAccountLookupVM>>(accounts));
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
            var lines = await expenseService.GetTransactionLinesByTranIdAsync(tranId, ct);
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

    [HttpPost]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(ExpenseEntryVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] ExpenseEntryVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var created = await expenseService.CreateAsync(_mapper.Map<ExpenseEntry>(model), GetCurrentUserName(), ct);
            return CreatedAtAction(nameof(GetById), new { sNo = created.SNo }, _mapper.Map<ExpenseEntryVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error creating expense entry");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPost("batch")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(ExpenseBatchSaveResultVM), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateBatch([FromBody] ExpenseBatchSaveVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var created = await expenseService.CreateBatchAsync(_mapper.Map<ExpenseBatchSaveRequest>(model), GetCurrentUserName(), ct);
            return StatusCode(201, _mapper.Map<ExpenseBatchSaveResultVM>(created));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error creating batch expense entries");
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPut("tran-id/{tranId}")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(ExpenseBatchSaveResultVM), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateByTranId(string tranId, [FromBody] ExpenseBatchSaveVM model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        try
        {
            var updated = await expenseService.UpdateByTranIdAsync(tranId, _mapper.Map<ExpenseBatchSaveRequest>(model), GetCurrentUserName(), ct);
            return Ok(_mapper.Map<ExpenseBatchSaveResultVM>(updated));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error updating expense transaction {TranId}", tranId);
            AddModelError(ex.Message);
            return BadRequest(new ValidationProblemDetails(ModelState));
        }
    }

    [HttpPut("{sNo:long}")]
    [Authorize(Policy = AuthPolicies.ManageAccountingPolicy)]
    [ProducesResponseType(typeof(ExpenseEntryVM), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(long sNo, [FromBody] ExpenseEntryVM model, CancellationToken ct)
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

        var existing = await expenseService.GetByIdAsync(sNo, ct);
        if (existing is null)
        {
            return NotFound(new { sNo });
        }

        try
        {
            var entry = _mapper.Map<ExpenseEntry>(model);
            entry.SNo = sNo;
            var updated = await expenseService.UpdateAsync(entry, GetCurrentUserName(), ct);
            return Ok(_mapper.Map<ExpenseEntryVM>(updated));
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error updating expense entry {SNo}", sNo);
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
            await expenseService.DeleteByTranIdAsync(tranId, GetCurrentUserName(), period, coyID, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error deleting expense transaction {TranId}", tranId);
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
        var existing = await expenseService.GetByIdAsync(sNo, ct);
        if (existing is null)
        {
            return NotFound(new { sNo });
        }

        try
        {
            await expenseService.DeleteAsync(sNo, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Validation error deleting expense entry {SNo}", sNo);
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
