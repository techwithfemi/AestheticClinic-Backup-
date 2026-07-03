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
