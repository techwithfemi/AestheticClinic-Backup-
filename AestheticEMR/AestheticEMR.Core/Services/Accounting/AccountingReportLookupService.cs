using AestheticEMR.Core.Models.Accounting;
using AestheticEMR.Core.Services.Accounting.Interfaces;
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
}
