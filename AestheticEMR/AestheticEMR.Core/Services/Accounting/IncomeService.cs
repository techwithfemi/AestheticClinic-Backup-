using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Dapper;
using DataAccess.DbAccess;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Accounting;

public class IncomeService(
    IEmrAppDefaultsService emrAppDefaultsService,
    ISqlDataAccess db,
    ILogger<IncomeService> logger) : IIncomeService
{
    private const string IncomesRemarks = "INCOMES";
    private const string JournalTranCat = "j";
    private const string AcctConn = "AccountingConnection";

    public async Task<PagedIncomeResult> GetPagedAsync(IncomeListQuery query, CancellationToken ct = default)
    {
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 10 : Math.Min(query.PageSize, 200);
        var search = query.Search?.Trim();

        var whereParts = new List<string> { "ISNULL(Remarks, '') = @Remarks" };
        var parameters = new DynamicParameters();
        parameters.Add("Remarks", IncomesRemarks);

        if (query.FromDate.HasValue && query.ToDate.HasValue)
        {
            whereParts.Add("TranDate BETWEEN @StartDate AND @EndDate");
            parameters.Add("StartDate", query.FromDate.Value.Date);
            parameters.Add("EndDate", query.ToDate.Value.Date);
        }
        else if (query.FromDate.HasValue)
        {
            whereParts.Add("TranDate >= @StartDate");
            parameters.Add("StartDate", query.FromDate.Value.Date);
        }
        else if (query.ToDate.HasValue)
        {
            whereParts.Add("TranDate <= @EndDate");
            parameters.Add("EndDate", query.ToDate.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            whereParts.Add(@"(
       Description LIKE @Search
    OR AccountName LIKE @Search
    OR AccountNo LIKE @Search
    OR TranNo LIKE @Search
    OR UserName LIKE @Search
    OR Period LIKE @Search
  )");
            parameters.Add("Search", $"%{search}%");
        }

        var where = $"WHERE {string.Join("\n  AND ", whereParts)}";

        parameters.Add("Skip", (page - 1) * pageSize);
        parameters.Add("Take", pageSize);

        var totalCount = (await db.LoadDataText<int, DynamicParameters>(
            $"SELECT COUNT(1) FROM vwTranx {where};",
            parameters,
            AcctConn)).FirstOrDefault();

        var items = (await db.LoadDataText<IncomeListItem, DynamicParameters>($@"
SELECT
    ROW_NUMBER() OVER (ORDER BY SNo) AS SN,
    TranDate,
    AccountName,
    AccountNo,
    'Debit' = CASE WHEN Amount > 0 THEN Amount ELSE 0 END,
    'Credit' = CASE WHEN Amount < 0 THEN ABS(Amount) ELSE 0 END,
    Description,
    TranNo,
    CatName2 AS TranCat,
    BillNo,
    CenterName AS CostCenter,
    EntryDate,
    Period,
    UserName,
    SNo,
    Remarks,
    CoyID,
    isClose
FROM vwTranx
{where}
ORDER BY SNo DESC
OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;",
            parameters,
            AcctConn)).ToList();

        return new PagedIncomeResult
        {
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public async Task<IncomeEntry?> GetByIdAsync(long sNo, CancellationToken ct = default)
    {
        return (await db.LoadDataText<IncomeEntry, dynamic>(@"
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
    Period,
    CoyID,
    Remarks
FROM vwTranx src
WHERE SNo = @SNo
  AND ISNULL(Remarks, '') = @Remarks;",
            new { SNo = sNo, Remarks = IncomesRemarks },
            AcctConn)).FirstOrDefault();
    }

    public async Task<List<IncomeEntry>> GetByTranIdAsync(string tranId, CancellationToken ct = default)
    {
        var normalizedTranId = tranId?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedTranId))
        {
            return [];
        }

        var rows = await db.LoadDataText<IncomeEntry, dynamic>(@"
WITH DebitLines AS (
    SELECT
        SNo, TranNo, TranDate, AccountNo, AccountName, Amount,
        Description, UserName, TranID, Period, CoyID, isClose,
        ROW_NUMBER() OVER (ORDER BY SNo) AS RowNum
    FROM vwTranx
    WHERE TranNo = @TranNo
      AND ISNULL(Remarks, '') = @Remarks
      AND Amount > 0
),
CreditLines AS (
    SELECT
        SNo, TranNo, TranDate, AccountNo, AccountName, Amount,
        Description, UserName, TranID, Period, CoyID, isClose,
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
    d.Period,
    d.CoyID,
    @Remarks AS Remarks
FROM DebitLines d
LEFT JOIN CreditLines c ON c.RowNum = d.RowNum
ORDER BY d.SNo;",
            new { TranNo = normalizedTranId, Remarks = IncomesRemarks },
            AcctConn);

        return rows.ToList();
    }

    public async Task<IncomeTranIdResult> GenerateTranIdAsync(CancellationToken ct = default)
    {
        var tranId = await GenerateTranNoAsync();
        if (string.IsNullOrWhiteSpace(tranId))
        {
            throw new InvalidOperationException("Unable to generate transaction id.");
        }

        return new IncomeTranIdResult { TranId = tranId };
    }

    public async Task<List<IncomeAccountLookup>> GetIncomeAccountsAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT DISTINCT
       LTRIM(RTRIM(AccountName)) AS AccountName,
       LTRIM(RTRIM(AccountNo))   AS AccountNo
FROM   vwAccountsInfoCombo
WHERE  (SUBSTRING(GroupID, 1, 1) = '4' OR SUBSTRING(GroupID, 1, 3) = '126')
ORDER BY AccountName;";

        var accounts = (await db.LoadDataText<IncomeAccountLookup, dynamic>(sql, new { }, AcctConn)).ToList();
        logger.LogInformation("Loaded {Count} income accounts for income dialog.", accounts.Count);
        return accounts;
    }

    public async Task<List<IncomeAccountLookup>> GetReceivingAccountsAsync(CancellationToken ct = default)
    {
        // Receiving accounts: Cash or Cheque (same as paying accounts in expenses)
        const string sql = @"
SELECT DISTINCT
       LTRIM(RTRIM(AccountName)) AS AccountName,
       LTRIM(RTRIM(AccountNo))   AS AccountNo
FROM   vwAccountsInfoCombo
WHERE  LTRIM(RTRIM(ISNULL(Remarks, ''))) IN ('Cheque','Cash')
ORDER BY AccountName;";

        var accounts = (await db.LoadDataText<IncomeAccountLookup, dynamic>(sql, new { }, AcctConn)).ToList();
        logger.LogInformation("Loaded {Count} receiving accounts for income dialog.", accounts.Count);
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

        var rows = await db.LoadDataText<TransactionLineRaw, dynamic>(sql, new { TranNo = tranId }, AcctConn);

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

    public async Task<IncomeEntry> CreateAsync(IncomeEntry entry, string currentUserName, CancellationToken ct = default)
    {
        var result = await CreateBatchAsync(new IncomeBatchSaveRequest { Entries = [entry] }, currentUserName, ct);
        return result.Entries.First();
    }

    public async Task<IncomeBatchSaveResult> CreateBatchAsync(IncomeBatchSaveRequest request, string currentUserName, CancellationToken ct = default)
    {
        if (request.Entries.Count == 0)
        {
            throw new InvalidOperationException("At least one income entry is required.");
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
            var description = string.IsNullOrWhiteSpace(entry.Description) ? "Income" : entry.Description.Trim();

            // For income: debit the receiving account (cash/bank), credit the income account
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
        await MarkTransactionAsIncomeAsync(tranId);

        var createdEntries = await GetByTranIdAsync(tranId, ct);
        if (createdEntries.Count == 0)
        {
            throw new InvalidOperationException("Income entry was created but could not be reloaded.");
        }

        return new IncomeBatchSaveResult { Entries = createdEntries };
    }

    public async Task<IncomeEntry> UpdateAsync(IncomeEntry entry, string currentUserName, CancellationToken ct = default)
    {
        if (entry.SNo is null || entry.SNo.Value <= 0)
        {
            throw new InvalidOperationException("Income entry id is required.");
        }

        var existing = await GetByIdAsync(entry.SNo.Value, ct);
        if (existing is null)
        {
            throw new InvalidOperationException("Income entry was not found.");
        }

        var tranId = existing.TranId?.Trim();
        if (string.IsNullOrWhiteSpace(tranId))
        {
            throw new InvalidOperationException("Income entry transaction id was not found.");
        }

        var result = await UpdateByTranIdAsync(tranId, new IncomeBatchSaveRequest
        {
            TranId = tranId,
            Entries = [entry]
        }, currentUserName, ct);

        return result.Entries.First();
    }

    public async Task<IncomeBatchSaveResult> UpdateByTranIdAsync(string tranId, IncomeBatchSaveRequest request, string currentUserName, CancellationToken ct = default)
    {
        var normalizedTranId = tranId?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedTranId))
        {
            throw new InvalidOperationException("Transaction id is required.");
        }

        if (request.Entries.Count == 0)
        {
            throw new InvalidOperationException("At least one income entry is required.");
        }

        var existingEntries = await GetByTranIdAsync(normalizedTranId, ct);
        if (existingEntries.Count == 0)
        {
            throw new InvalidOperationException("Income transaction was not found.");
        }

        await EnsureEditableAsync(existingEntries.First().SNo ?? 0, ct);

        var deleteSource = request.Entries.FirstOrDefault(x =>
            string.Equals(x.TranId?.Trim(), normalizedTranId, StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(x.Period)
            && !string.IsNullOrWhiteSpace(x.CoyID));

        if (deleteSource is null)
        {
            throw new InvalidOperationException("Period and CoyID are required from the UI row/record for update delete operation.");
        }

        await DeleteByTranIdAsync(normalizedTranId, currentUserName, deleteSource.Period!.Trim(), deleteSource.CoyID!.Trim(), ct);

        return await CreateBatchAsync(new IncomeBatchSaveRequest
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
            throw new InvalidOperationException("Income entry was not found.");
        }

        var tranId = existing.TranId?.Trim();
        if (string.IsNullOrWhiteSpace(tranId))
        {
            throw new InvalidOperationException("Income entry transaction id was not found.");
        }

        if (string.IsNullOrWhiteSpace(existing.Period) || string.IsNullOrWhiteSpace(existing.CoyID))
        {
            throw new InvalidOperationException("Period and CoyID are required from the current income record for delete operation.");
        }

        await DeleteByTranIdAsync(tranId, existing.UserName ?? string.Empty, existing.Period.Trim(), existing.CoyID.Trim(), ct);
    }

    public async Task DeleteByTranIdAsync(string tranId, string currentUserName, string period, string coyID, CancellationToken ct = default)
    {
        var normalizedTranId = tranId?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedTranId))
        {
            throw new InvalidOperationException("Transaction id is required.");
        }

        if (string.IsNullOrWhiteSpace(period) || string.IsNullOrWhiteSpace(coyID))
        {
            throw new InvalidOperationException("Period and CoyID are required for delete operation.");
        }

        var existingEntries = await GetByTranIdAsync(normalizedTranId, ct);
        if (existingEntries.Count == 0)
        {
            throw new InvalidOperationException("Income transaction was not found.");
        }

        await EnsureEditableAsync(existingEntries.First().SNo ?? 0, ct);

        await db.SaveData("Deletetranxaction", new
        {
            Period = period.Trim(),
            CoyID = coyID.Trim(),
            TranNo = normalizedTranId,
            Username = string.IsNullOrWhiteSpace(currentUserName) ? (existingEntries.First().UserName ?? string.Empty) : currentUserName
        }, AcctConn);

        var remaining = (await db.LoadDataText<int, dynamic>(@"
SELECT COUNT(1)
FROM vwTranx
WHERE TranNo = @TranNo
  AND ISNULL(Remarks, '') = @Remarks;",
            new { TranNo = normalizedTranId, Remarks = IncomesRemarks },
            AcctConn)).FirstOrDefault();

        if (remaining > 0)
        {
            throw new InvalidOperationException("Income entry delete did not complete.");
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

    private async Task ValidateAsync(IncomeEntry entry, CancellationToken ct)
    {
        if (entry.TranDate == default)
        {
            throw new InvalidOperationException("Tran Date is required.");
        }

        if (string.IsNullOrWhiteSpace(entry.AccountDebit))
        {
            throw new InvalidOperationException("Income account is required.");
        }

        if (string.IsNullOrWhiteSpace(entry.AccountCredit))
        {
            throw new InvalidOperationException("Receiving account is required.");
        }

        if (entry.Amount <= 0)
        {
            throw new InvalidOperationException("Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(entry.Description))
        {
            throw new InvalidOperationException("Description is required.");
        }

        var debitExists = (await db.LoadDataText<int, dynamic>(@"
SELECT COUNT(1)
FROM vwAccountsInfoCombo
WHERE LTRIM(RTRIM(AccountNo)) = @AccountNo
  AND SUBSTRING(ISNULL(GroupID, ''), 1, 1) = '4';",
            new { AccountNo = entry.AccountDebit.Trim() },
            AcctConn)).FirstOrDefault() > 0;

        if (!debitExists)
        {
            throw new InvalidOperationException("Selected income account was not found.");
        }

        var creditExists = (await db.LoadDataText<int, dynamic>(@"
SELECT COUNT(1)
FROM vwAccountsInfoCombo
WHERE LTRIM(RTRIM(AccountNo)) = @AccountNo
  AND LTRIM(RTRIM(ISNULL(Remarks, ''))) IN ('Cheque', 'Cash');",
            new { AccountNo = entry.AccountCredit.Trim() },
            AcctConn)).FirstOrDefault() > 0;

        if (!creditExists)
        {
            throw new InvalidOperationException("Selected receiving account was not found.");
        }
    }

    private async Task EnsureEditableAsync(long sNo, CancellationToken ct)
    {
        if (sNo <= 0)
        {
            throw new InvalidOperationException("Income entry was not found.");
        }

        var row = (await db.LoadDataText<IncomeEditStateRow, dynamic>(@"
SELECT TOP 1
    ISNULL(isClose, 0) AS IsClose
FROM vwTranx
WHERE SNo = @SNo
  AND ISNULL(Remarks, '') = @Remarks;",
            new { SNo = sNo, Remarks = IncomesRemarks },
            AcctConn)).FirstOrDefault();

        if (row is null)
        {
            throw new InvalidOperationException("Income entry was not found.");
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
                await db.SaveData("Deletetranxaction", new
                {
                    Period = period,
                    CoyID = coyId,
                    TranNo = tranId,
                    Username = currentUserName
                }, AcctConn);
                throw new InvalidOperationException("Account posting failed because the transaction did not balance.");
            }
        }
    }

    private async Task MarkTransactionAsIncomeAsync(string tranId)
    {
        await db.SaveDataText(@"
UPDATE Tranxaction
SET Remarks = @Remarks
WHERE TranNo = @TranNo
  AND ISNULL(Remarks, '') = '';",
            new { TranNo = tranId, Remarks = IncomesRemarks },
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

    private sealed class IncomeEditStateRow
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
