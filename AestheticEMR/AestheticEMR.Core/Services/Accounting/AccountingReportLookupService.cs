using AestheticEMR.Core.Models.Accounting;
using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using DataAccess.DbAccess;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Accounting;

public class AccountingReportLookupService(ISqlDataAccess db, ILogger<AccountingReportLookupService> logger) : IAccountingReportLookupService
{
    private const string AcctConn = "AccountingConnection";

    public async Task<IEnumerable<vwProfitAndLossHeadersList>> GetProfitAndLossHeadersAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT DISTINCT
    LTRIM(RTRIM(ItemName)) AS ItemName,
    LTRIM(RTRIM(GroupID)) AS GroupID
FROM dbo.vwProfitAndLossHeadersList
WHERE ISNULL(LTRIM(RTRIM(GroupID)), '') <> ''
ORDER BY LTRIM(RTRIM(GroupID));";

        try
        {
            return await db.LoadDataText<vwProfitAndLossHeadersList, object>(sql, new { }, AcctConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading profit and loss headers.");
            throw;
        }
    }

    public async Task<IEnumerable<vwBalanceSheetHeader>> GetBalanceSheetHeadersAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT DISTINCT
    LTRIM(RTRIM(ItemName)) AS ItemName,
    LTRIM(RTRIM(RptType)) AS RptType,
    LTRIM(RTRIM(Period)) AS Period,
    LTRIM(RTRIM(CoyID)) AS CoyID
FROM dbo.vwBalanceSheetHeaders
WHERE ISNULL(LTRIM(RTRIM(ItemName)), '') <> ''
ORDER BY LTRIM(RTRIM(RptType)), LTRIM(RTRIM(ItemName));";

        try
        {
            return await db.LoadDataText<vwBalanceSheetHeader, object>(sql, new { }, AcctConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading balance sheet headers.");
            throw;
        }
    }

    public async Task<IEnumerable<AccountingReportYearLookup>> GetGeneralLedgerYearsAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT DISTINCT
    LTRIM(RTRIM(PeriodYr)) AS PeriodYr
FROM dbo.vwClosedAndOpenPeriods
WHERE ISNULL(LTRIM(RTRIM(PeriodYr)), '') <> ''
ORDER BY PeriodYr DESC;";

        try
        {
            return await db.LoadDataText<AccountingReportYearLookup, object>(sql, new { }, AcctConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading general ledger years.");
            throw;
        }
    }

    public async Task<IEnumerable<AccountingReportPeriodLookup>> GetGeneralLedgerPeriodsAsync(string coyID, string year, CancellationToken ct = default)
    {
        const string sql = @"
SELECT
    p.Period,
    p.PeriodVal,
    p.IsClose,
    p.PrdClose
FROM
(
    SELECT DISTINCT
        LTRIM(RTRIM(Period)) AS Period,
        CAST(PeriodVal AS nvarchar(20)) AS PeriodVal,
        CAST(ISNULL(isClose, 0) AS bit) AS IsClose,
        PrdClose
    FROM dbo.vwClosedAndOpenPeriods
    WHERE CoyID = @CoyID
      AND PeriodYr = @Year
      AND ISNULL(LTRIM(RTRIM(Period)), '') <> ''
) p
ORDER BY p.Period DESC;";

        try
        {
            return await db.LoadDataText<AccountingReportPeriodLookup, object>(sql, new { CoyID = coyID.Trim(), Year = year.Trim() }, AcctConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading general ledger periods for CoyID {CoyID} and Year {Year}.", coyID, year);
            throw;
        }
    }

    public async Task<IEnumerable<AccountingLedgerLookup>> GetGeneralLedgerLedgersAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT
    l.LedgerCode,
    l.Ledger
FROM
(
    SELECT '' AS LedgerCode, '' AS Ledger
    UNION
    SELECT DISTINCT
        LTRIM(RTRIM(LedgerCode)) AS LedgerCode,
        LTRIM(RTRIM(Ledger)) AS Ledger
    FROM dbo.ledgerCategory
) l
ORDER BY l.Ledger;";

        try
        {
            return await db.LoadDataText<AccountingLedgerLookup, object>(sql, new { }, AcctConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading general ledger ledgers.");
            throw;
        }
    }

    public async Task<IEnumerable<AccountingAccountLookup>> GetGeneralLedgerAccountsAsync(string coyID, string period, string ledgerCode, CancellationToken ct = default)
    {
        var normalizedLedgerCode = ledgerCode.Trim();
        string sql;
        object parameters;

        if (string.Equals(normalizedLedgerCode, "GL", StringComparison.OrdinalIgnoreCase))
        {
            sql = @"
SELECT
    a.AccountNo,
    a.AccountName
FROM
(
    SELECT '(ALL)' AS AccountNo, '(ALL)' AS AccountName
    UNION
    SELECT DISTINCT
        LTRIM(RTRIM(AccountNo)) AS AccountNo,
        LTRIM(RTRIM(AccountName)) AS AccountName
    FROM dbo.vwGL
    WHERE CoyID = @CoyID
      AND Period = @Period
) a
ORDER BY a.AccountName;";
            parameters = new { CoyID = coyID.Trim(), Period = period.Trim() };
        }
        else if (string.Equals(normalizedLedgerCode, "PL", StringComparison.OrdinalIgnoreCase))
        {
            sql = @"
SELECT
    a.AccountNo,
    a.AccountName
FROM
(
    SELECT '(ALL)' AS AccountNo, '(ALL)' AS AccountName
    UNION
    SELECT DISTINCT
        LTRIM(RTRIM(AccountNo)) AS AccountNo,
        LTRIM(RTRIM(AccountName)) AS AccountName
    FROM dbo.vwGLforRptPL
    WHERE CoyID = @CoyID
      AND Period = @Period
) a
ORDER BY a.AccountName;";
            parameters = new { CoyID = coyID.Trim(), Period = period.Trim() };
        }
        else
        {
            sql = @"
SELECT
    a.AccountNo,
    a.AccountName
FROM
(
    SELECT '(ALL)' AS AccountNo, '(ALL)' AS AccountName
    UNION
    SELECT DISTINCT
        LTRIM(RTRIM(AccountNo)) AS AccountNo,
        LTRIM(RTRIM(AccountName)) AS AccountName
    FROM dbo.vwGLforRpt
    WHERE CoyID = @CoyID
      AND Period = @Period
      AND LedgerCode = @LedgerCode
) a
ORDER BY a.AccountName;";
            parameters = new { CoyID = coyID.Trim(), Period = period.Trim(), LedgerCode = normalizedLedgerCode };
        }

        try
        {
            return await db.LoadDataText<AccountingAccountLookup, object>(sql, parameters, AcctConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading general ledger accounts for CoyID {CoyID}, Period {Period}, LedgerCode {LedgerCode}.", coyID, period, ledgerCode);
            throw;
        }
    }

    public async Task<string?> GetCompanyNameAsync(string coyID, CancellationToken ct = default)
    {
        const string sql = @"
SELECT TOP 1 LTRIM(RTRIM(Coyname)) AS Coyname
FROM dbo.Companies
WHERE CoyID = @CoyID;";

        try
        {
            var results = await db.LoadDataText<CompanyNameRow, object>(sql, new { CoyID = coyID.Trim() }, AcctConn);
            return results.FirstOrDefault()?.Coyname;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading company name for CoyID {CoyID}.", coyID);
            throw;
        }
    }

    private sealed class CompanyNameRow
    {
        public string? Coyname { get; set; }
    }
}
