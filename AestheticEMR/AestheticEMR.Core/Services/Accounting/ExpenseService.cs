using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using AestheticEMR.Core.Services.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Dapper;
using DataAccess.DbAccess;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Accounting;

public class ExpenseService(
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
        var fromDate = query.FromDate?.Date;
        var toDateExclusive = query.ToDate?.Date.AddDays(1);

        var where = @" WHERE ISNULL(Remarks, '') = @Remarks ";
        var parameters = new DynamicParameters();
        parameters.Add("Remarks", ExpensesRemarks);

        if (!string.IsNullOrWhiteSpace(search))
        {
            where += @"
 AND (
      ISNULL(Description, '') LIKE @Search
   OR ISNULL(AccountName, '') LIKE @Search
   OR ISNULL(TranNo, '') LIKE @Search
   OR ISNULL(UserName, '') LIKE @Search
 )";
            parameters.Add("Search", $"%{search}%");
        }

        if (fromDate is not null)
        {
            where += " AND TranDate >= @FromDate";
            parameters.Add("FromDate", fromDate.Value);
        }

        if (toDateExclusive is not null)
        {
            where += " AND TranDate < @ToDateExclusive";
            parameters.Add("ToDateExclusive", toDateExclusive.Value);
        }

        parameters.Add("Skip", (page - 1) * pageSize);
        parameters.Add("Take", pageSize);

        var totalCount = (await db.LoadDataText<int, DynamicParameters>(
            $"SELECT COUNT(DISTINCT TranNo) FROM vwTranx {where};",
            parameters,
            AcctConn)).FirstOrDefault();

        var items = (await db.LoadDataText<ExpenseListItem, DynamicParameters>($@"
WITH ExpensesByTran AS (
    SELECT
        TranNo,
        TranDate,
        Amount,
        AccountNo,
        AccountName,
        Description,
        UserName,
        TranID,
        isClose,
        ROW_NUMBER() OVER (PARTITION BY TranNo ORDER BY CASE WHEN Amount > 0 THEN 0 ELSE 1 END, SNo) AS LineNum
    FROM vwTranx
    {where}
)
SELECT
    (SELECT MIN(SNo) FROM vwTranx WHERE TranNo = e.TranNo AND ISNULL(Remarks, '') = @Remarks) AS SNo,
    e.TranDate,
    (SELECT TOP 1 AccountNo FROM ExpensesByTran WHERE TranNo = e.TranNo AND Amount > 0 ORDER BY LineNum) AS AccountDebit,
    (SELECT TOP 1 AccountNo FROM ExpensesByTran WHERE TranNo = e.TranNo AND Amount < 0 ORDER BY LineNum) AS AccountCredit,
    (SELECT TOP 1 AccountName FROM ExpensesByTran WHERE TranNo = e.TranNo AND Amount > 0 ORDER BY LineNum) AS DebitAccountName,
    (SELECT TOP 1 AccountName FROM ExpensesByTran WHERE TranNo = e.TranNo AND Amount < 0 ORDER BY LineNum) AS CreditAccountName,
    ABS(e.Amount) AS Amount,
    e.Description,
    1 AS IsPost,
    e.IsClose,
    e.UserName,
    e.TranID AS TranId,
    @Remarks AS Remarks
FROM (
    SELECT DISTINCT
        TranNo,
        TranDate,
        Amount,
        Description,
        IsClose,
        UserName,
        TranID
    FROM ExpensesByTran
    WHERE Amount > 0
) e
ORDER BY e.TranDate DESC, e.TranNo DESC
OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;",
            parameters,
            AcctConn)).ToList();

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
        return (await db.LoadDataText<ExpenseEntry, dynamic>(@"
SELECT TOP 1
    SNo,
    TranDate,
    (SELECT TOP 1 AccountNo FROM vwTranx WHERE TranNo = src.TranNo AND Amount > 0 ORDER BY SNo) AS AccountDebit,
    (SELECT TOP 1 AccountNo FROM vwTranx WHERE TranNo = src.TranNo AND Amount < 0 ORDER BY SNo) AS AccountCredit,
    (SELECT TOP 1 AccountName FROM vwTranx WHERE TranNo = src.TranNo AND Amount > 0 ORDER BY SNo) AS DebitAccountName,
    (SELECT TOP 1 AccountName FROM vwTranx WHERE TranNo = src.TranNo AND Amount < 0 ORDER BY SNo) AS CreditAccountName,
    ABS(CASE WHEN Amount < 0 THEN Amount * -1 ELSE Amount END) AS Amount,
    Description,
    1 AS IsPost,
    isClose AS IsClose,
    UserName,
    TranID AS TranId,
    Remarks
FROM vwTranx src
WHERE SNo = @SNo
  AND ISNULL(Remarks, '') = @Remarks;",
            new { SNo = sNo, Remarks = ExpensesRemarks },
            AcctConn)).FirstOrDefault();
    }

    public async Task<List<ExpenseEntry>> GetByTranIdAsync(string tranId, CancellationToken ct = default)
    {
        var normalizedTranId = tranId?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedTranId))
        {
            return [];
        }

        var rows = await db.LoadDataText<ExpenseEntry, dynamic>(@"
WITH DebitLines AS (
    SELECT
        SNo,
        TranNo,
        TranDate,
        AccountNo,
        AccountName,
        Amount,
        Description,
        UserName,
        TranID,
        isClose,
        ROW_NUMBER() OVER (ORDER BY SNo) AS RowNum
    FROM vwTranx
    WHERE TranNo = @TranNo
      AND ISNULL(Remarks, '') = @Remarks
      AND Amount > 0
),
CreditLines AS (
    SELECT
        SNo,
        TranNo,
        TranDate,
        AccountNo,
        AccountName,
        Amount,
        Description,
        UserName,
        TranID,
        isClose,
        ROW_NUMBER() OVER (ORDER BY SNo) AS RowNum
    FROM vwTranx
    WHERE TranNo = @TranNo
      AND ISNULL(Remarks, '') = @Remarks
      AND Amount < 0
)
SELECT
    d.SNo,
    d.TranDate,
    d.AccountNo AS AccountDebit,
    ISNULL(c.AccountNo, '') AS AccountCredit,
    d.AccountName AS DebitAccountName,
    ISNULL(c.AccountName, '') AS CreditAccountName,
    d.Amount AS Amount,
    COALESCE(NULLIF(LTRIM(RTRIM(d.Description)), ''), c.Description, '') AS Description,
    1 AS IsPost,
    d.isClose AS IsClose,
    d.UserName,
    d.TranID AS TranId,
    @Remarks AS Remarks
FROM DebitLines d
LEFT JOIN CreditLines c ON c.RowNum = d.RowNum
ORDER BY d.SNo;",
            new { TranNo = normalizedTranId, Remarks = ExpensesRemarks },
            AcctConn);

        return rows.ToList();
    }

    public async Task<ExpenseTranIdResult> GenerateTranIdAsync(CancellationToken ct = default)
    {
        var tranId = await GenerateTranNoAsync();
        if (string.IsNullOrWhiteSpace(tranId))
        {
            throw new InvalidOperationException("Unable to generate transaction id.");
        }

        return new ExpenseTranIdResult { TranId = tranId };
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
WHERE  LTRIM(RTRIM(ISNULL(Remarks, ''))) IN ('Cheque','Cash')
ORDER BY AccountName;";

        var accounts = (await db.LoadDataText<ExpenseAccountLookup, dynamic>(sql, new { }, AcctConn)).ToList();
        logger.LogInformation("Loaded {Count} paying accounts for expenses dialog.", accounts.Count);
        return accounts;
    }

    public async Task<List<JournalLine>> GetTransactionLinesByTranIdAsync(string tranId, CancellationToken ct = default)
    {
        const string sql = @"
SELECT  SNo,
        TranNo,
        AccountNo,
        AccountName,
        Amount,
        Description,
        TranDate
FROM    dbo.Tranxaction
WHERE   TranNo = @TranNo
ORDER BY SNo;";

        var rows = await db.LoadDataText<TransactionLineRaw, dynamic>(
            sql,
            new { TranNo = tranId },
            AcctConn);

        return rows.Select(row => new JournalLine
        {
            AccountNo = row.AccountNo ?? string.Empty,
            AccountName = row.AccountName ?? string.Empty,
            Debit = row.Amount > 0 ? row.Amount : 0,
            Credit = row.Amount < 0 ? -row.Amount : 0,
            Description = row.Description,
            TranDate = row.TranDate
        }).ToList();
    }

    public async Task<ExpenseEntry> CreateAsync(ExpenseEntry entry, string currentUserName, CancellationToken ct = default)
    {
        var result = await CreateBatchAsync(new ExpenseBatchSaveRequest { Entries = [entry] }, currentUserName, ct);
        return result.Entries.First();
    }

    public async Task<ExpenseBatchSaveResult> CreateBatchAsync(ExpenseBatchSaveRequest request, string currentUserName, CancellationToken ct = default)
    {
        if (request.Entries.Count == 0)
        {
            throw new InvalidOperationException("At least one expense entry is required.");
        }

        foreach (var entry in request.Entries)
        {
            await ValidateAsync(entry, ct);
        }

        var defaults = await emrAppDefaultsService.GetAsync(ct);
        EnsureAccountingPostingEnabled(defaults);

        var tranId = string.IsNullOrWhiteSpace(request.TranId)
            ? await GenerateTranNoAsync()
            : request.TranId.Trim();

        if (string.IsNullOrWhiteSpace(tranId))
        {
            throw new InvalidOperationException("Unable to generate transaction id.");
        }

        var coyId = defaults.Get("CoyID", "0001");
        var costCenter = defaults.Get("AcctCostCenter", "0001");
        var periods = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var entry in request.Entries)
        {
            var tranDate = entry.TranDate;
            var period = await ResolvePeriodAsync(tranDate, ct);
            periods.Add(period);
            var description = string.IsNullOrWhiteSpace(entry.Description) ? "Expense" : entry.Description.Trim();

            await CallInsertTranxactionAsync(
                tranId,
                entry.AccountDebit.Trim(),
                entry.Amount,
                description,
                tranDate,
                costCenter,
                period,
                coyId,
                currentUserName,
                ct);

            await CallInsertTranxactionAsync(
                tranId,
                entry.AccountCredit.Trim(),
                -entry.Amount,
                description,
                tranDate,
                costCenter,
                period,
                coyId,
                currentUserName,
                ct);
        }

        await EnsureBalancedAsync(periods, coyId, tranId, currentUserName);
        await MarkTransactionAsExpenseAsync(tranId);

        var createdEntries = await GetByTranIdAsync(tranId, ct);
        if (createdEntries.Count == 0)
        {
            throw new InvalidOperationException("Expense entry was created but could not be reloaded.");
        }

        return new ExpenseBatchSaveResult { Entries = createdEntries };
    }

    public async Task<ExpenseEntry> UpdateAsync(ExpenseEntry entry, string currentUserName, CancellationToken ct = default)
    {
        if (entry.SNo is null || entry.SNo.Value <= 0)
        {
            throw new InvalidOperationException("Expense entry id is required.");
        }

        var existing = await GetByIdAsync(entry.SNo.Value, ct);
        if (existing is null)
        {
            throw new InvalidOperationException("Expense entry was not found.");
        }

        var tranId = existing.TranId?.Trim();
        if (string.IsNullOrWhiteSpace(tranId))
        {
            throw new InvalidOperationException("Expense entry transaction id was not found.");
        }

        var result = await UpdateByTranIdAsync(tranId, new ExpenseBatchSaveRequest
        {
            TranId = tranId,
            Entries = [entry]
        }, currentUserName, ct);

        return result.Entries.First();
    }

    public async Task<ExpenseBatchSaveResult> UpdateByTranIdAsync(string tranId, ExpenseBatchSaveRequest request, string currentUserName, CancellationToken ct = default)
    {
        var normalizedTranId = tranId?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedTranId))
        {
            throw new InvalidOperationException("Transaction id is required.");
        }

        if (request.Entries.Count == 0)
        {
            throw new InvalidOperationException("At least one expense entry is required.");
        }

        var existingEntries = await GetByTranIdAsync(normalizedTranId, ct);
        if (existingEntries.Count == 0)
        {
            throw new InvalidOperationException("Expense transaction was not found.");
        }

        await EnsureEditableAsync(existingEntries.First().SNo ?? 0, ct);
        await DeleteByTranIdAsync(normalizedTranId, currentUserName, ct);

        return await CreateBatchAsync(new ExpenseBatchSaveRequest
        {
            TranId = normalizedTranId,
            Entries = request.Entries
        }, currentUserName, ct);
    }

    public async Task DeleteAsync(long sNo, CancellationToken ct = default)
    {
        var existing = await GetByIdAsync(sNo, ct);
        if (existing is null)
        {
            throw new InvalidOperationException("Expense entry was not found.");
        }

        var tranId = existing.TranId?.Trim();
        if (string.IsNullOrWhiteSpace(tranId))
        {
            throw new InvalidOperationException("Expense entry transaction id was not found.");
        }

        await DeleteByTranIdAsync(tranId, existing.UserName ?? string.Empty, ct);
    }

    public async Task DeleteByTranIdAsync(string tranId, string currentUserName, CancellationToken ct = default)
    {
        var normalizedTranId = tranId?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedTranId))
        {
            throw new InvalidOperationException("Transaction id is required.");
        }

        var existingEntries = await GetByTranIdAsync(normalizedTranId, ct);
        if (existingEntries.Count == 0)
        {
            throw new InvalidOperationException("Expense transaction was not found.");
        }

        await EnsureEditableAsync(existingEntries.First().SNo ?? 0, ct);

        await db.SaveData("Deletetranxaction", new
        {
            TranID = normalizedTranId,
            TranNo = normalizedTranId,
            userName = currentUserName
        }, AcctConn);

        var remaining = (await db.LoadDataText<int, dynamic>(@"
SELECT COUNT(1)
FROM vwTranx
WHERE TranNo = @TranNo
  AND ISNULL(Remarks, '') = @Remarks;",
            new { TranNo = normalizedTranId, Remarks = ExpensesRemarks },
            AcctConn)).FirstOrDefault();

        if (remaining > 0)
        {
            throw new InvalidOperationException("Expense entry delete did not complete.");
        }
    }

    private static void EnsureAccountingPostingEnabled(EmrAppDefaults defaults)
    {
        var acctPostOn = string.Equals(defaults.Get("AcctPostOn", "NO"), "YES", StringComparison.OrdinalIgnoreCase)
            || string.Equals(defaults.Get("AcctPostOn", "false"), "true", StringComparison.OrdinalIgnoreCase);

        if (!acctPostOn)
        {
            throw new InvalidOperationException("Accounting posting is disabled.");
        }
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

        var debitAccountNo = entry.AccountDebit.Trim();
        var creditAccountNo = entry.AccountCredit.Trim();

        var debitExists = (await db.LoadDataText<int, dynamic>(@"
SELECT COUNT(1)
FROM vwAccountsInfoCombo
WHERE LTRIM(RTRIM(AccountNo)) = @AccountNo
  AND SUBSTRING(ISNULL(GroupID, ''), 1, 1) = '5';",
            new { AccountNo = debitAccountNo },
            AcctConn)).FirstOrDefault() > 0;

        if (!debitExists)
        {
            throw new InvalidOperationException("Selected expense account was not found.");
        }

        var creditExists = (await db.LoadDataText<int, dynamic>(@"
SELECT COUNT(1)
FROM vwAccountsInfoCombo
WHERE LTRIM(RTRIM(AccountNo)) = @AccountNo
  AND LTRIM(RTRIM(ISNULL(Remarks, ''))) IN ('Cheque', 'Cash');",
            new { AccountNo = creditAccountNo },
            AcctConn)).FirstOrDefault() > 0;

        if (!creditExists)
        {
            throw new InvalidOperationException("Selected paying account was not found.");
        }
    }

    private async Task EnsureEditableAsync(long sNo, CancellationToken ct)
    {
        if (sNo <= 0)
        {
            throw new InvalidOperationException("Expense entry was not found.");
        }

        var row = (await db.LoadDataText<ExpenseEditStateRow, dynamic>(@"
SELECT TOP 1
    ISNULL(isClose, 0) AS IsClose
FROM vwTranx
WHERE SNo = @SNo
  AND ISNULL(Remarks, '') = @Remarks;",
            new { SNo = sNo, Remarks = ExpensesRemarks },
            AcctConn)).FirstOrDefault();

        if (row is null)
        {
            throw new InvalidOperationException("Expense entry was not found.");
        }

        if (row.IsClose)
        {
            throw new InvalidOperationException("This item cannot be changed because the accounting period is already closed.");
        }
    }

    private async Task EnsureBalancedAsync(IEnumerable<string> periods, string coyId, string tranId, string currentUserName)
    {
        foreach (var period in periods.Where(static x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var balanceRows = await db.LoadDataText<decimal, dynamic>(
                "SELECT dbo.TranBalance(@Period, @CoyID) AS Amount",
                new { Period = period, CoyID = coyId },
                AcctConn);

            var balance = balanceRows.FirstOrDefault();
            if (balance != 0m)
            {
                await db.SaveData("Deletetranxaction", new { TranID = tranId, TranNo = tranId, userName = currentUserName }, AcctConn);
                throw new InvalidOperationException("Account posting failed because the transaction did not balance.");
            }
        }
    }

    private async Task MarkTransactionAsExpenseAsync(string tranId)
    {
        await db.SaveDataText(@"
UPDATE Tranxaction
SET Remarks = @Remarks
WHERE TranNo = @TranNo
  AND ISNULL(Remarks, '') = '';",
            new { TranNo = tranId, Remarks = ExpensesRemarks },
            AcctConn);
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
            EntryDate = tranDate,
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

    private async Task<string> ResolvePeriodAsync(DateTime tranDate, CancellationToken ct)
    {
        foreach (var parameters in GetPeriodParameterCandidates(tranDate))
        {
            try
            {
                var rows = await db.LoadData<GetPeriodRaw, dynamic>("getPeriod", parameters, AcctConn);
                var value = rows.FirstOrDefault()?.Period;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }
            catch
            {
                // Try next parameter signature; fall back below.
            }
        }

        return $"{tranDate.Month:D2}/{tranDate.Year}";
    }

    private static IEnumerable<dynamic> GetPeriodParameterCandidates(DateTime tranDate)
    {
        yield return new { TranDate = tranDate };
        yield return new { Date = tranDate };
        yield return new { PayDate = tranDate };
        yield return new { StartDate = tranDate };
    }

    private sealed class GetTranIdRaw
    {
        public string? TranID { get; set; }
        public string? TranNo { get; set; }
        public string? Id { get; set; }
    }

    private sealed class ExpenseEditStateRow
    {
        public bool IsClose { get; set; }
    }

    private sealed class TransactionLineRaw
    {
        public long SNo { get; set; }
        public string? TranNo { get; set; }
        public string? AccountNo { get; set; }
        public string? AccountName { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TranDate { get; set; }
    }

    private sealed class GetPeriodRaw
    {
        public string? Period { get; set; }
        public string? AcctPeriod { get; set; }

        public string? Value => Period ?? AcctPeriod;
    }
}
