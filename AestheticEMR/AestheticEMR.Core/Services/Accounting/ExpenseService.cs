using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Accounting;
using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using AestheticEMR.Core.Services.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using DataAccess.DbAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Accounting;

public class ExpenseService(
    AccountingDbContext accountingDbContext,
    IEmrAppDefaultsService emrAppDefaultsService,
    ISqlDataAccess db,
    ILogger<ExpenseService> logger) : IExpenseService
{
    private const string ExpensesRemarks = "EXPENSES";
    private const string JournalTranCat = "j";
    private const string AcctConn = "AccountingConnection";

    public async Task<PagedExpenseResult> GetPagedAsync(ExpenseListQuery query, CancellationToken ct = default)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 10 : Math.Min(query.PageSize, 200);
        var search = query.Search?.Trim();
        var viewMode = (query.ViewMode ?? "all").Trim().ToLowerInvariant();
        var fromDate = query.FromDate?.Date;
        var toDateExclusive = query.ToDate?.Date.AddDays(1);

        var expenseQuery = accountingDbContext.vwTranxJournalTemps
            .AsNoTracking()
            .Where(x => (x.Remarks ?? string.Empty) == ExpensesRemarks);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search}%";
            expenseQuery = expenseQuery.Where(x =>
                EF.Functions.Like(x.Description ?? string.Empty, pattern) ||
                EF.Functions.Like(x.AccountNameDebit ?? string.Empty, pattern) ||
                EF.Functions.Like(x.AccountNameCredit ?? string.Empty, pattern) ||
                EF.Functions.Like(x.AccountDebit ?? string.Empty, pattern) ||
                EF.Functions.Like(x.AccountCredit ?? string.Empty, pattern) ||
                EF.Functions.Like(x.UserName ?? string.Empty, pattern));
        }

        if (fromDate is not null)
        {
            expenseQuery = expenseQuery.Where(x => x.TranDate >= fromDate.Value);
        }

        if (toDateExclusive is not null)
        {
            expenseQuery = expenseQuery.Where(x => x.TranDate < toDateExclusive.Value);
        }

        expenseQuery = viewMode switch
        {
            "posted" => expenseQuery.Where(x => x.IsPost),
            "unposted" => expenseQuery.Where(x => !x.IsPost),
            _ => expenseQuery
        };

        var totalCount = await expenseQuery.CountAsync(ct);
        var items = await expenseQuery
            .OrderByDescending(x => x.TranDate)
            .ThenByDescending(x => x.SNo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ExpenseListItem
            {
                SNo = x.SNo,
                TranDate = x.TranDate,
                AccountDebit = x.AccountDebit,
                AccountCredit = x.AccountCredit,
                DebitAccountName = x.AccountNameDebit,
                CreditAccountName = x.AccountNameCredit,
                Amount = x.Amount,
                Description = x.Description,
                IsPost = x.IsPost,
                IsClose = x.isClose ?? false,
                UserName = x.UserName,
                TranId = x.TranID,
                Remarks = x.Remarks
            })
            .ToListAsync(ct);

        return new PagedExpenseResult
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<ExpenseEntry?> GetByIdAsync(long sNo, CancellationToken ct = default)
    {
        return await accountingDbContext.vwTranxJournalTemps
            .AsNoTracking()
            .Where(x => x.SNo == sNo && (x.Remarks ?? string.Empty) == ExpensesRemarks)
            .Select(x => new ExpenseEntry
            {
                SNo = x.SNo,
                TranDate = x.TranDate,
                AccountDebit = x.AccountDebit,
                AccountCredit = x.AccountCredit,
                DebitAccountName = x.AccountNameDebit,
                CreditAccountName = x.AccountNameCredit,
                Amount = x.Amount,
                Description = x.Description ?? string.Empty,
                IsPost = x.IsPost,
                PostDirectly = x.IsPost,
                IsClose = x.isClose ?? false,
                UserName = x.UserName,
                TranId = x.TranID,
                Remarks = x.Remarks
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<ExpenseAccountLookup>> GetExpenseAccountsAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT DISTINCT
       LTRIM(RTRIM(AccountName)) AS AccountName,
       LTRIM(RTRIM(AccountNo))   AS AccountNo
FROM   vwAccountsInfoCombo
WHERE  SUBSTRING(GroupID, 1, 1) = '5'
ORDER BY AccountName;";

        var accounts = (await db.LoadDataText<ExpenseAccountLookup, dynamic>(sql, new { }, AcctConn)).ToList();
        logger.LogInformation("Loaded {Count} expense accounts for expenses dialog.", accounts.Count);
        return accounts;
    }

    public async Task<List<ExpenseAccountLookup>> GetPayingAccountsAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT DISTINCT
       LTRIM(RTRIM(AccountName)) AS AccountName,
       LTRIM(RTRIM(AccountNo))   AS AccountNo
FROM   vwAccountsInfoCombo
WHERE  Remarks IN ('Cheque','Cash')
ORDER BY AccountName;";

        var accounts = (await db.LoadDataText<ExpenseAccountLookup, dynamic>(sql, new { }, AcctConn)).ToList();
        logger.LogInformation("Loaded {Count} paying accounts for expenses dialog.", accounts.Count);
        return accounts;
    }

    public async Task<ExpenseEntry> CreateAsync(ExpenseEntry entry, string currentUserName, CancellationToken ct = default)
    {
        await ValidateAsync(entry, ct);

        var defaults = await emrAppDefaultsService.GetAsync(ct);
        var expense = new TranxactionJournalTemp
        {
            TranDate = entry.TranDate,
            AccountDebit = entry.AccountDebit.Trim(),
            AccountCredit = entry.AccountCredit.Trim(),
            CoyID = defaults.Get("CoyID", "0001"),
            Amount = entry.Amount,
            Description = entry.Description.Trim(),
            TranCat = JournalTranCat,
            IsPost = false,
            Remarks = ExpensesRemarks,
            UserName = currentUserName
        };

        await accountingDbContext.TranxactionJournalTemps.AddAsync(expense, ct);
        await accountingDbContext.SaveChangesAsync(ct);

        if (entry.PostDirectly)
        {
            await PostExpenseAsync(expense.SNo, currentUserName, defaults, ct);
        }

        return (await GetByIdAsync(expense.SNo, ct))!;
    }

    public async Task<ExpenseEntry> UpdateAsync(ExpenseEntry entry, string currentUserName, CancellationToken ct = default)
    {
        if (entry.SNo is null || entry.SNo.Value <= 0)
        {
            throw new InvalidOperationException("Expense entry id is required.");
        }

        await ValidateAsync(entry, ct);
        await EnsureEditableAsync(entry.SNo.Value, allowUnpostedOnly: true, ct);

        var existing = await accountingDbContext.TranxactionJournalTemps.FirstOrDefaultAsync(x => x.SNo == entry.SNo.Value, ct);
        if (existing is null)
        {
            throw new InvalidOperationException("Expense entry was not found.");
        }

        existing.TranDate = entry.TranDate;
        existing.AccountDebit = entry.AccountDebit.Trim();
        existing.AccountCredit = entry.AccountCredit.Trim();
        existing.Amount = entry.Amount;
        existing.Description = entry.Description.Trim();
        existing.Remarks = ExpensesRemarks;
        existing.TranCat = JournalTranCat;
        existing.UserName = currentUserName;
        existing.IsPost = false;
        existing.TranID = null;

        await accountingDbContext.SaveChangesAsync(ct);

        if (entry.PostDirectly)
        {
            var defaults = await emrAppDefaultsService.GetAsync(ct);
            await PostExpenseAsync(existing.SNo, currentUserName, defaults, ct);
        }

        return (await GetByIdAsync(existing.SNo, ct))!;
    }

    public async Task DeleteAsync(long sNo, CancellationToken ct = default)
    {
        await EnsureEditableAsync(sNo, allowUnpostedOnly: true, ct);

        var existing = await accountingDbContext.TranxactionJournalTemps.FirstOrDefaultAsync(x => x.SNo == sNo, ct);
        if (existing is null)
        {
            throw new InvalidOperationException("Expense entry was not found.");
        }

        accountingDbContext.TranxactionJournalTemps.Remove(existing);
        await accountingDbContext.SaveChangesAsync(ct);
    }

    private async Task ValidateAsync(ExpenseEntry entry, CancellationToken ct)
    {
        if (entry.TranDate == default)
        {
            throw new InvalidOperationException("Tran Date is required.");
        }

        if (string.IsNullOrWhiteSpace(entry.AccountDebit))
        {
            throw new InvalidOperationException("Expense account is required.");
        }

        if (string.IsNullOrWhiteSpace(entry.AccountCredit))
        {
            throw new InvalidOperationException("Paying account is required.");
        }

        if (entry.Amount <= 0)
        {
            throw new InvalidOperationException("Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(entry.Description))
        {
            throw new InvalidOperationException("Description is required.");
        }

        var debitExists = await accountingDbContext.vwAccountsInfos
            .AsNoTracking()
            .AnyAsync(x => x.AccountNo == entry.AccountDebit.Trim(), ct);

        if (!debitExists)
        {
            throw new InvalidOperationException("Selected expense account was not found.");
        }

        var creditExists = await accountingDbContext.vwAccountsInfos
            .AsNoTracking()
            .AnyAsync(x => x.AccountNo == entry.AccountCredit.Trim(), ct);

        if (!creditExists)
        {
            throw new InvalidOperationException("Selected paying account was not found.");
        }
    }

    private async Task EnsureEditableAsync(long sNo, bool allowUnpostedOnly, CancellationToken ct)
    {
        var row = await accountingDbContext.vwTranxJournalTemps
            .AsNoTracking()
            .Where(x => x.SNo == sNo && (x.Remarks ?? string.Empty) == ExpensesRemarks)
            .Select(x => new { x.IsPost, IsClose = x.isClose ?? false })
            .FirstOrDefaultAsync(ct);

        if (row is null)
        {
            throw new InvalidOperationException("Expense entry was not found.");
        }

        if (row.IsClose)
        {
            throw new InvalidOperationException("This item cannot be changed because the accounting period is already closed.");
        }

        if (allowUnpostedOnly && row.IsPost)
        {
            throw new InvalidOperationException("Posted expense entries cannot be changed from this screen.");
        }
    }

    private async Task PostExpenseAsync(long sNo, string currentUserName, EmrAppDefaults defaults, CancellationToken ct)
    {
        var expense = await accountingDbContext.TranxactionJournalTemps.FirstOrDefaultAsync(x => x.SNo == sNo, ct);
        if (expense is null)
        {
            throw new InvalidOperationException("Expense entry was not found.");
        }

        if (expense.IsPost)
        {
            return;
        }

        var acctPostOn = string.Equals(defaults.Get("AcctPostOn", "NO"), "YES", StringComparison.OrdinalIgnoreCase)
            || string.Equals(defaults.Get("AcctPostOn", "false"), "true", StringComparison.OrdinalIgnoreCase);

        if (!acctPostOn)
        {
            throw new InvalidOperationException("Accounting posting is disabled.");
        }

        var tranNo = await GenerateTranNoAsync();
        if (string.IsNullOrWhiteSpace(tranNo))
        {
            throw new InvalidOperationException("Unable to generate transaction number for posting.");
        }

        var period = GetPeriodFromDate(expense.TranDate, defaults);
        var coyId = string.IsNullOrWhiteSpace(expense.CoyID) ? defaults.Get("CoyID", "0001") : expense.CoyID;
        var costCenter = defaults.Get("AcctCostCenter", "0001");
        var description = string.IsNullOrWhiteSpace(expense.Description) ? "Expense" : expense.Description.Trim();

        await CallInsertTranxactionAsync(tranNo, expense.AccountDebit, expense.Amount, description, expense.TranDate, costCenter, period, coyId, currentUserName, ct);
        await CallInsertTranxactionAsync(tranNo, expense.AccountCredit, -expense.Amount, description, expense.TranDate, costCenter, period, coyId, currentUserName, ct);

        var balanceRows = await db.LoadDataText<decimal, dynamic>("SELECT dbo.TranBalance(@Period, @CoyID) AS Amount", new { Period = period, CoyID = coyId }, AcctConn);
        var balance = balanceRows.FirstOrDefault();
        if (balance != 0m)
        {
            await db.SaveData("deleteTranxaction", new { Period = string.Empty, CoyID = coyId, TranNo = tranNo, userName = currentUserName }, AcctConn);
            throw new InvalidOperationException("Account posting failed because the transaction did not balance.");
        }

        expense.IsPost = true;
        expense.TranID = tranNo;
        expense.UserName = currentUserName;
        await accountingDbContext.SaveChangesAsync(ct);
    }

    private async Task CallInsertTranxactionAsync(
        string tranNo,
        string accountNo,
        decimal amount,
        string description,
        DateTime tranDate,
        string costCenter,
        string period,
        string coyId,
        string userName,
        CancellationToken ct)
    {
        await db.SaveData("InsertTranxaction", new
        {
            TranID = tranNo,
            AccountNo = accountNo,
            TranNo = tranNo,
            TranDate = tranDate,
            CostCenterID = costCenter,
            Amount = amount,
            Description = description,
            TranCat = JournalTranCat,
            EntryDate = DateTime.Now,
            Period = period,
            CoyID2 = coyId,
            UserName = userName,
            SNoID = 0,
            BillNO = string.Empty,
            Reversed = false,
            ReversedPair = 0
        }, AcctConn);
    }

    private async Task<string> GenerateTranNoAsync()
    {
        try
        {
            var rows = await db.LoadData<GetTranIdRaw, dynamic>("getTranID", new { }, AcctConn);
            var row = rows.FirstOrDefault();
            var tranId = row?.TranID ?? row?.TranNo ?? row?.Id;
            if (!string.IsNullOrWhiteSpace(tranId))
            {
                return tranId.Trim();
            }
        }
        catch
        {
            // fall back to max+1 below
        }

        var result = await db.LoadDataText<long, dynamic>("SELECT ISNULL(MAX(CAST(TranNo AS BIGINT)), 0) + 1 FROM vwTranxNo;", new { }, AcctConn);
        return result.FirstOrDefault().ToString();
    }

    private static string GetPeriodFromDate(DateTime date, EmrAppDefaults defaults)
    {
        var periodType = defaults.Get("AcctPeriodType", "MTHLY");
        if (string.Equals(periodType, "YRLY", StringComparison.OrdinalIgnoreCase))
        {
            return date.Year.ToString();
        }

        return $"{date.Year}-{date.Month:D2}";
    }

    private sealed class GetTranIdRaw
    {
        public string? TranID { get; set; }
        public string? TranNo { get; set; }
        public string? Id { get; set; }
    }
}
