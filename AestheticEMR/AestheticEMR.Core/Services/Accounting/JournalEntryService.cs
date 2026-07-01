using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using AestheticEMR.Core.Services.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using DataAccess.DbAccess;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Accounting;

/// <summary>
/// Journal Entry service backed by the Accounting DB via Dapper.
/// Mirrors the VB frmJournal / Tran.saveJournal flow against the
/// InsertTranxactionJournal / UpdateTranxactionJournal stored procedures.
/// All read/write calls go through <see cref="ISqlDataAccess"/> using the
/// "AccountingConnection" connection id.
///
/// Runtime defaults (AUTO_TRAN_NO, CoyID, AcctCostCenter, AcctPeriodType)
/// are sourced from <see cref="IEmrAppDefaultsService"/>, which mirrors the
/// VB AppSettings / emrAppDefaults.json flow.
/// </summary>
public class JournalEntryService(
    ISqlDataAccess db,
    IEmrAppDefaultsService emrDefaults,
    ILogger<JournalEntryService> logger) : IJournalEntryService
{
    private const string AcctConn = "AccountingConnection";

    public async Task<PagedJournalResult> GetPagedAsync(JournalListQuery query, CancellationToken ct = default)
    {
        var search = query.Search?.Trim();
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 10 : Math.Min(query.PageSize, 200);

        var filters = new List<string> { "1=1" };
        if (!string.IsNullOrWhiteSpace(search))
        {
            filters.Add("TranNo LIKE @Search");
        }
        if (query.FromDate is not null)
        {
            filters.Add("TranDate >= @FromDate");
        }
        if (query.ToDate is not null)
        {
            filters.Add("TranDate < @ToDate");
        }

        var whereClause = string.Join(" AND ", filters);

        var countSql = $"SELECT COUNT(*) FROM vwTranxNo WHERE {whereClause};";
        var pageSql = $@"
SELECT  t.TranNo,
        MIN(t.TranDate) AS TranDate,
        COUNT(*)         AS LineCount,
        ISNULL(SUM(CASE WHEN j.Amount > 0 THEN j.Amount ELSE 0 END), 0) AS TotalDebit,
        ISNULL(SUM(CASE WHEN j.Amount < 0 THEN -j.Amount ELSE 0 END), 0) AS TotalCredit
FROM    vwTranxNo t
LEFT JOIN dbo.Tranxaction j ON j.TranNo = t.TranNo
WHERE   {whereClause}
GROUP BY t.TranNo
ORDER BY MIN(t.TranDate) DESC, t.TranNo DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var parameters = new
        {
            Search = $"%{search}%",
            FromDate = query.FromDate,
            ToDate = query.ToDate,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        };

        var totalRows = await db.LoadDataText<int, dynamic>(countSql, parameters, AcctConn);
        var items = await db.LoadDataText<JournalListItemRaw, dynamic>(pageSql, parameters, AcctConn);

        return new PagedJournalResult
        {
            TotalCount = totalRows.FirstOrDefault(),
            Page = page,
            PageSize = pageSize,
            Items = items.Select(MapListItem).ToList()
        };
    }

    public async Task<PagedJournalLinesResult> GetPagedLinesAsync(JournalListLineQuery query, CancellationToken ct = default)
    {
        var search = query.Search?.Trim();
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 10 : Math.Min(query.PageSize, 200);

        // Default: when no search is supplied, restrict to the supplied TranDate
        // (the front-end sends today by default). When the user types a search,
        // ignore the date filter so they can find any TranNo across all dates.
        var filters = new List<string> { "1=1" };
        if (!string.IsNullOrWhiteSpace(search))
        {
            filters.Add("(TranNo LIKE @Search OR AccountName LIKE @Search OR Description LIKE @Search)");
        }
        else if (query.TranDate is not null)
        {
            filters.Add("TranDate >= @TranDateFrom AND TranDate < @TranDateTo");
        }

        var whereClause = string.Join(" AND ", filters);

        // Mirrors the user's preferred projection:
        //   select ROW_NUMBER() OVER (ORDER BY SNo) AS SN, TranDate, AccountName, AccountNo,
        //          'Debit'  = case when Amount > 0 then Amount else 0 end,
        //          'Credit' = case when Amount < 0 then abs(Amount) else 0 end,
        //          Description, TranNo, CatName2 as TranCat, BillNo, CenterName as CostCenter,
        //          EntryDate, Period, UserName, SNo, Remarks, CoyID, isClose
        //   from vwTranx
        var countSql = $"SELECT COUNT(*) FROM vwTranx WHERE {whereClause};";
        var pageSql = $@"
SELECT  ROW_NUMBER() OVER (ORDER BY v.SNo) AS SN,
        v.TranDate,
        v.AccountName,
        v.AccountNo,
        CASE WHEN v.Amount > 0 THEN v.Amount ELSE 0 END                              AS Debit,
        CASE WHEN v.Amount < 0 THEN ABS(v.Amount) ELSE 0 END                          AS Credit,
        v.Description,
        v.TranNo,
        v.CatName2                                                                   AS TranCat,
        v.BillNo,
        v.CenterName                                                                 AS CostCenter,
        v.EntryDate,
        v.Period,
        v.UserName,
        v.SNo,
        v.Remarks,
        v.CoyID,
        v.isClose
FROM    vwTranx v
WHERE   {whereClause}
ORDER BY v.SNo
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var parameters = new
        {
            Search = $"%{search}%",
            TranDateFrom = query.TranDate?.Date,
            TranDateTo = query.TranDate?.Date.AddDays(1),
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        };

        var totalRows = await db.LoadDataText<int, dynamic>(countSql, parameters, AcctConn);
        var rows = await db.LoadDataText<JournalListLineRaw, dynamic>(pageSql, parameters, AcctConn);

        return new PagedJournalLinesResult
        {
            TotalCount = totalRows.FirstOrDefault(),
            Page = page,
            PageSize = pageSize,
            Items = rows.Select(MapListLine).ToList()
        };
    }

    public async Task<JournalEntry?> GetByTranNoAsync(string tranNo, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(tranNo))
        {
            return null;
        }

        var headerSql = @"
SELECT  TOP 1 TranNo,
        TranDate,
        CostCenterID AS CostCenterId
FROM    dbo.Tranxaction
WHERE   TranNo = @TranNo
ORDER BY TranDate;";

        var linesSql = @"
SELECT  AccountNo,
        AccountName,
        Amount,
        Description,
        TranDate,
        CostCenterID AS CostCenterId
FROM    dbo.Tranxaction
WHERE   TranNo = @TranNo
ORDER BY TranNo, TranDate;";

        var headerRows = await db.LoadDataText<JournalHeaderRaw, dynamic>(headerSql, new { TranNo = tranNo }, AcctConn);
        var header = headerRows.FirstOrDefault();
        if (header is null)
        {
            return null;
        }

        var lineRows = await db.LoadDataText<JournalLineRaw, dynamic>(linesSql, new { TranNo = tranNo }, AcctConn);
        var lines = lineRows.Select(MapLineFromRow).ToList();

        return new JournalEntry
        {
            TranNo = header.TranNo,
            TranDate = header.TranDate,
            CostCenterId = header.CostCenterId ?? string.Empty,
            Lines = lines
        };
    }

    public async Task<string> GenerateNextTranNoAsync(CancellationToken ct = default)
    {
        // Mirrors genTranID in legacy VB, preferring getTranID sproc.
        var defaults = await emrDefaults.GetAsync(ct);
        var autoTranNo = defaults.Get("AUTO_TRAN_NO", "YES");
        if (!string.Equals(autoTranNo, "YES", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

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
            // fall back to MAX+1 query below
        }

        var sql = "SELECT ISNULL(MAX(CAST(TranNo AS BIGINT)), 0) + 1 FROM vwTranxNo;";
        var result = await db.LoadDataText<long, dynamic>(sql, new { }, AcctConn);
        var next = result.FirstOrDefault();
        return next.ToString();
    }

    public async Task<List<JournalAccountLookup>> GetAccountsAsync(CancellationToken ct = default)
    {
        var sql = @"
SELECT DISTINCT
        LTRIM(RTRIM(AccountNo))   AS AccountNo,
        LTRIM(RTRIM(AccountName)) AS AccountName
FROM    dbo.ChartOfAccountMaster
WHERE   ISNULL(LTRIM(RTRIM(AccountNo)), '') <> ''
  AND   ISNULL(LTRIM(RTRIM(AccountName)), '') <> ''
ORDER BY AccountName;";
        var rows = await db.LoadDataText<JournalAccountLookup, dynamic>(sql, new { }, AcctConn);
        return rows.ToList();
    }

    public async Task<List<JournalCostCenterLookup>> GetCostCentersAsync(CancellationToken ct = default)
    {
        var sql = @"
SELECT  CenterID   AS CenterId,
        CenterName
FROM    dbo.CostCenters
ORDER BY CenterName;";
        var rows = await db.LoadDataText<JournalCostCenterLookup, dynamic>(sql, new { }, AcctConn);
        return rows.ToList();
    }

    public async Task<JournalEntry> CreateAsync(JournalEntry entry, string currentUser, CancellationToken ct = default)
    {
        Validate(entry);

        // If AUTO_TRAN_NO = NO and the caller did not supply a TranNo, this is a hard error.
        var defaults = await emrDefaults.GetAsync(ct);
        var autoTranNo = defaults.Get("AUTO_TRAN_NO", "YES");

        if (string.Equals(autoTranNo, "NO", StringComparison.OrdinalIgnoreCase) && string.IsNullOrWhiteSpace(entry.TranNo))
        {
            throw new InvalidOperationException("Tran No is required (AUTO_TRAN_NO = NO).");
        }

        // Always re-check duplicate TranNo against the live table (mirrors the VB pre-check).
        var existsSql = "SELECT COUNT(*) FROM vwTranxNo WHERE TranNo = @TranNo;";
        var existing = await db.LoadDataText<int, dynamic>(existsSql, new { TranNo = entry.TranNo }, AcctConn);
        if (existing.FirstOrDefault() > 0)
        {
            throw new InvalidOperationException($"Error! This Transaction No ({entry.TranNo}) already exists in the database.");
        }

        // VB calls TranBalanceJournal(period, coy) at the end of saveJournal and bails if != 0.
        // We mirror that with the same scalar function in the Accounting DB.
        var period = GetPeriodFromDate(entry.TranDate, defaults);
        var coyId = defaults.Get("CoyID", "0001");

        await InsertLinesAsync(entry, currentUser, defaults, coyId);

        await EnsureBalancedAsync(period, coyId);

        return (await GetByTranNoAsync(entry.TranNo, ct))!;
    }

    public async Task<JournalEntry> UpdateAsync(JournalEntry entry, string currentUser, CancellationToken ct = default)
    {
        Validate(entry);

        // Mirrors UpdateTranxactionJournal's "delete-then-reinsert" pattern from the VB code.
        var deleteSql = "DELETE FROM dbo.Tranxaction WHERE TranNo = @TranNo;";
        await db.SaveDataText(deleteSql, new { TranNo = entry.TranNo }, AcctConn);

        var defaults = await emrDefaults.GetAsync(ct);
        var coyId = defaults.Get("CoyID", "0001");
        var period = GetPeriodFromDate(entry.TranDate, defaults);

        await InsertLinesAsync(entry, currentUser, defaults, coyId);
        await EnsureBalancedAsync(period, coyId);

        return (await GetByTranNoAsync(entry.TranNo, ct))!;
    }

    public async Task DeleteAsync(string tranNo, string currentUser, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(tranNo))
        {
            throw new ArgumentException("TranNo is required", nameof(tranNo));
        }

        const string deleteProc = "deleteTranxaction";
        await db.SaveData(deleteProc, new { TranNo = tranNo }, AcctConn);
        logger.LogInformation("Journal entry {TranNo} deleted by {User}", tranNo, currentUser);
    }

    private async Task InsertLinesAsync(JournalEntry entry, string currentUser, EmrAppDefaults defaults, string coyId)
    {
        // Mirrors the per-row InsertTranxactionJournal loop in Tran.saveJournal.
        // If both Debit and Credit are set on a row, two rows are inserted (Dr + Cr).
        const string insertProc = "insertTranxaction";
        var defaultCostCenter = defaults.Get("AcctCostCenter", "0001");

        foreach (var line in entry.Lines)
        {
            // The VB saveJournal accepts empty/null CostCenter and falls back to AcctCostCenter.
            var costCenter = string.IsNullOrWhiteSpace(entry.CostCenterId) ? defaultCostCenter : entry.CostCenterId;
            var period = GetPeriodFromDate(line.TranDate, defaults);

            if (line.Debit > 0 && line.Credit > 0)
            {
                // Debit side (positive)
                await db.SaveData(insertProc, new
                {
                    TranNo = entry.TranNo,
                    TranDate = line.TranDate,
                    TranID = entry.TranNo,
                    AccountNo = line.AccountNo,
                    CostCenterID = costCenter,
                    Amount = line.Debit,
                    Description = line.Description ?? "NIL",
                    TranCat = "Journal",
                    EntryDate = DateTime.Today,
                    Period2 = period,
                    CoyID2 = coyId,
                    UserName = currentUser
                }, AcctConn);

                // Credit side (negative)
                await db.SaveData(insertProc, new
                {
                    TranNo = entry.TranNo,
                    TranDate = line.TranDate,
                    TranID = entry.TranNo,
                    AccountNo = line.AccountNo,
                    CostCenterID = costCenter,
                    Amount = -line.Credit,
                    Description = line.Description ?? "NIL",
                    TranCat = "Journal",
                    EntryDate = DateTime.Today,
                    Period2 = period,
                    CoyID2 = coyId,
                    UserName = currentUser
                }, AcctConn);
            }
            else
            {
                var amount = line.Debit > 0 ? line.Debit : -line.Credit;
                await db.SaveData(insertProc, new
                {
                    TranNo = entry.TranNo,
                    TranDate = line.TranDate,
                    TranID = entry.TranNo,
                    AccountNo = line.AccountNo,
                    CostCenterID = costCenter,
                    Amount = amount,
                    Description = line.Description ?? "NIL",
                    TranCat = "Journal",
                    EntryDate = DateTime.Today,
                    Period2 = period,
                    CoyID2 = coyId,
                    UserName = currentUser
                }, AcctConn);
            }
        }
    }

    private async Task EnsureBalancedAsync(string period, string coyId)
    {
        // VB: SELECT dbo.TranBalanceJournal(period, coy) after inserts.
        // Throws if the result is not 0.
        var sql = "SELECT dbo.TranBalanceJournal(@Period, @CoyID);";
        var rows = await db.LoadDataText<int, dynamic>(sql, new { Period = period, CoyID = coyId }, AcctConn);
        var result = rows.FirstOrDefault();
        if (result != 0)
        {
            throw new InvalidOperationException(
                "Transaction not balanced! Check your Tran Dates — they should all belong to one Period.");
        }
    }

    private static void Validate(JournalEntry entry)
    {
        if (string.IsNullOrWhiteSpace(entry.TranNo))
        {
            throw new InvalidOperationException("Tran No is required.");
        }
        if (entry.TranDate == default)
        {
            throw new InvalidOperationException("Tran Date is required.");
        }
        if (entry.Lines is null || entry.Lines.Count == 0)
        {
            throw new InvalidOperationException("At least one journal line is required.");
        }

        var totalDebit = entry.Lines.Sum(l => l.Debit);
        var totalCredit = entry.Lines.Sum(l => l.Credit);
        if (totalDebit != totalCredit)
        {
            throw new InvalidOperationException(
                $"Transaction is not balanced. Total Debit ({totalDebit:n2}) must equal Total Credit ({totalCredit:n2}).");
        }
        if (totalDebit == 0)
        {
            throw new InvalidOperationException("Cannot save a journal with zero totals.");
        }

        foreach (var line in entry.Lines)
        {
            if (string.IsNullOrWhiteSpace(line.AccountNo))
            {
                throw new InvalidOperationException("Every line must have an Account.");
            }
            if (line.TranDate == default)
            {
                throw new InvalidOperationException("Every line must have a Tran Date.");
            }
            if (line.Debit < 0 || line.Credit < 0)
            {
                throw new InvalidOperationException("Debit and Credit values cannot be negative.");
            }
            if (line.Debit == 0 && line.Credit == 0)
            {
                throw new InvalidOperationException($"Line for {line.AccountNo}: either Debit or Credit must be non-zero.");
            }
        }
    }

    /// <summary>
    /// Compute the financial period key (e.g. "2025-11") honouring AcctPeriodType.
    /// Defaults to monthly (MTHLY) to match the VB behaviour for the seeded default.
    /// </summary>
    private static string GetPeriodFromDate(DateTime date, EmrAppDefaults defaults)
    {
        var periodType = defaults.Get("AcctPeriodType", "MTHLY");
        if (string.Equals(periodType, "YRLY", StringComparison.OrdinalIgnoreCase))
        {
            return date.Year.ToString();
        }
        return $"{date.Year}-{date.Month:D2}";
    }

    private static JournalListItem MapListItem(JournalListItemRaw r) => new()
    {
        TranNo = r.TranNo ?? string.Empty,
        TranDate = r.TranDate,
        LineCount = r.LineCount,
        TotalDebit = r.TotalDebit,
        TotalCredit = r.TotalCredit
    };

    private static JournalListLine MapListLine(JournalListLineRaw r) => new()
    {
        SN = r.SN,
        TranDate = r.TranDate,
        AccountName = r.AccountName ?? string.Empty,
        AccountNo = r.AccountNo ?? string.Empty,
        Debit = r.Debit,
        Credit = r.Credit,
        Description = r.Description,
        TranNo = r.TranNo ?? string.Empty,
        TranCat = r.TranCat,
        BillNo = r.BillNo,
        CostCenter = r.CostCenter,
        EntryDate = r.EntryDate,
        Period = r.Period,
        UserName = r.UserName,
        SNo = r.SNo,
        Remarks = r.Remarks,
        CoyID = r.CoyID ?? string.Empty,
        IsClose = r.IsClose
    };

    private static JournalLine MapLineFromRow(JournalLineRaw r)
    {
        var dr = r.Amount >= 0 ? r.Amount : 0;
        var cr = r.Amount < 0 ? -r.Amount : 0;
        return new JournalLine
        {
            AccountNo = r.AccountNo ?? string.Empty,
            AccountName = r.AccountName ?? string.Empty,
            Debit = dr,
            Credit = cr,
            Description = r.Description,
            TranDate = r.TranDate
        };
    }

    private class JournalListItemRaw
    {
        public string? TranNo { get; set; }
        public DateTime TranDate { get; set; }
        public int LineCount { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
    }

    /// <summary>
    /// Raw row shape from the <c>vwTranx</c> projection in
    /// <see cref="GetPagedLinesAsync"/>. Dapper matches columns to
    /// properties case-insensitively.
    /// </summary>
    private class JournalListLineRaw
    {
        public long SN { get; set; }
        public DateTime TranDate { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNo { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string? Description { get; set; }
        public string? TranNo { get; set; }
        public string? TranCat { get; set; }
        public string? BillNo { get; set; }
        public string? CostCenter { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Period { get; set; }
        public string? UserName { get; set; }
        public long SNo { get; set; }
        public string? Remarks { get; set; }
        public string? CoyID { get; set; }
        public bool IsClose { get; set; }
    }

    private class JournalHeaderRaw
    {
        public string TranNo { get; set; } = string.Empty;
        public DateTime TranDate { get; set; }
        public string? CostCenterId { get; set; }
    }

    private class JournalLineRaw
    {
        public string AccountNo { get; set; } = string.Empty;
        public string? AccountName { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TranDate { get; set; }
        public string? CostCenterId { get; set; }
    }

    private class GetTranIdRaw
    {
        public string? TranID { get; set; }
        public string? TranNo { get; set; }
        public string? Id { get; set; }
    }
}