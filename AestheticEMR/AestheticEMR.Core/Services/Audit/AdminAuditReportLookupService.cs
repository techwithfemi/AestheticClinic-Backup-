using AestheticEMR.Core.Services.Audit.Interfaces;
using DataAccess.DbAccess;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Audit;

public class AdminAuditReportLookupService(ISqlDataAccess db, ILogger<AdminAuditReportLookupService> logger) : IAdminAuditReportLookupService
{
    private const string DefaultConn = "DefaultConnection";

    public async Task<IEnumerable<AdminAuditReportRow>> GetReportRowsAsync(DateTime fromDate, DateTime toDate, string filterType, string? filterValue, string? searchTerm, CancellationToken ct = default)
    {
        var normalizedFilterType = string.IsNullOrWhiteSpace(filterType) ? "ALL" : filterType.Trim().ToUpperInvariant();
        var normalizedSearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null : searchTerm.Trim();
        var normalizedFilterValue = string.IsNullOrWhiteSpace(filterValue) ? null : filterValue.Trim();

        const string sql = @"
SELECT
    [ID] AS Id,
    [Date] AS [Date],
    [Time] AS [Time],
    [UserAction] AS [UserAction],
    [OriginalAction] AS [OriginalAction],
    [Remarks] AS [Remarks],
    [Src] AS [Src],
    [Employee] AS [Employee],
    [UserName] AS [UserName],
    [TranCode] AS [TranCode],
    [Module] AS [Module]
FROM [vwAudiTrail]
WHERE
    [Date] >= @FromDate AND [Date] <= @ToDate
    AND
    (
        @SearchTerm IS NULL
        OR LTRIM(RTRIM(ISNULL([TranCode], ''))) LIKE @SearchPattern
        OR LTRIM(RTRIM(ISNULL([Module], ''))) LIKE @SearchPattern
        OR LTRIM(RTRIM(ISNULL([Employee], ''))) LIKE @SearchPattern
        OR LTRIM(RTRIM(ISNULL([UserAction], ''))) LIKE @SearchPattern
        OR LTRIM(RTRIM(ISNULL([OriginalAction], ''))) LIKE @SearchPattern
        OR LTRIM(RTRIM(ISNULL([Remarks], ''))) LIKE @SearchPattern
        OR LTRIM(RTRIM(ISNULL([Src], ''))) LIKE @SearchPattern
    )
    AND
    (
        @FilterType = 'ALL'
        OR (@FilterType = 'USER' AND LTRIM(RTRIM(ISNULL([UserName], ''))) = @FilterValue)
        OR (@FilterType = 'MODULE' AND LTRIM(RTRIM(ISNULL([Module], ''))) = @FilterValue)
    )
ORDER BY [ID] DESC;";

        try
        {
            return await db.LoadDataText<AdminAuditReportRow, object>(sql, new
            {
                FromDate = fromDate.Date,
                ToDate = toDate.Date,
                FilterType = normalizedFilterType,
                FilterValue = normalizedFilterValue,
                SearchTerm = normalizedSearchTerm,
                SearchPattern = normalizedSearchTerm is null ? null : $"%{normalizedSearchTerm}%"
            }, DefaultConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading admin audit report rows.");
            throw;
        }
    }

    public async Task<IEnumerable<AdminAuditReportUserLookup>> GetUsersAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT DISTINCT
    LTRIM(RTRIM(ISNULL(FullName, UserName))) AS FullName,
    LTRIM(RTRIM(UserName)) AS UserName,
    LTRIM(RTRIM(ISNULL(FullName, UserName))) + ' [' + LTRIM(RTRIM(UserName)) + ']' AS DisplayText
FROM dbo.AspNetUsers
WHERE ISNULL(IsEnabled, 0) = 1
  AND ISNULL(LTRIM(RTRIM(UserName)), '') <> ''
ORDER BY LTRIM(RTRIM(ISNULL(FullName, UserName))), LTRIM(RTRIM(UserName));";

        try
        {
            return await db.LoadDataText<AdminAuditReportUserLookup, object>(sql, new { }, DefaultConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading admin audit report users.");
            throw;
        }
    }

    public async Task<IEnumerable<AdminAuditReportModuleLookup>> GetModulesAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT DISTINCT
    LTRIM(RTRIM([Name])) AS [Name]
FROM dbo.AspNetRoles
WHERE ISNULL(LTRIM(RTRIM([Name])), '') <> ''
ORDER BY LTRIM(RTRIM([Name]));";

        try
        {
            return await db.LoadDataText<AdminAuditReportModuleLookup, object>(sql, new { }, DefaultConn);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading admin audit report modules.");
            throw;
        }
    }
}
