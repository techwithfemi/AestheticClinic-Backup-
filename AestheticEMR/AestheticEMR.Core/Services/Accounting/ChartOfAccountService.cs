using AestheticEMR.Core.Services.Accounting.Interfaces;
using AestheticEMR.Core.Services.Accounting.Models;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using DataAccess.DbAccess;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Accounting;

public class ChartOfAccountService(
    ISqlDataAccess db,
    IEmrAppDefaultsService emrDefaults,
    ILogger<ChartOfAccountService> logger) : IChartOfAccountService
{
    private const string AcctConn = "AccountingConnection";

    public async Task<PagedChartOfAccountResult> GetPagedAsync(ChartOfAccountListQuery query, CancellationToken ct = default)
    {
        var search = query.Search?.Trim();
        var page = query.Page <= 0 ? 1 : query.Page;
        var pageSize = query.PageSize <= 0 ? 10 : Math.Min(query.PageSize, 200);

        var sortBy = (query.SortBy ?? string.Empty).Trim().ToLowerInvariant();
        var sortDirection = string.Equals(query.SortDirection, "desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
        var sortColumn = sortBy switch
        {
            "accountname" => "e.AccountName",
            "groupname" => "e.GroupName",
            "accountdesc" => "e.AccountDesc",
            "accountopamt" => "e.AccountOpAmt",
            "accountclamt" => "e.AccountClAmt",
            "accountno" => "e.AccountNo",
            _ => "e.AccountNo"
        };

        var filters = new List<string> { "1=1" };
        if (!string.IsNullOrWhiteSpace(search))
        {
            filters.Add("(e.AccountNo LIKE @Search OR e.AccountName LIKE @Search OR e.GroupName LIKE @Search OR e.AccountDesc LIKE @Search)");
        }

        var whereClause = string.Join(" AND ", filters);

        var baseSql = @"
WITH Enriched AS
(
    SELECT
        m.SNo,
        LTRIM(RTRIM(m.AccountNo)) AS AccountNo,
        LTRIM(RTRIM(m.AccountName)) AS AccountName,
        LTRIM(RTRIM(m.GroupID)) AS GroupID,
        COALESCE(NULLIF(LTRIM(RTRIM(i.GroupName)), ''), LTRIM(RTRIM(g.GroupName)), '') AS GroupName,
        m.AccountDesc,
        COALESCE(i.AccountOpAmt, m.AccountOpAmt, 0) AS AccountOpAmt,
        COALESCE(i.AccountClAmt, m.AccountClAmt, 0) AS AccountClAmt
    FROM dbo.ChartOfAccountMaster m
    OUTER APPLY
    (
        SELECT TOP 1
            v.GroupName,
            v.AccountOpAmt,
            v.AccountClAmt
        FROM vwAccountsInfo v
        WHERE LTRIM(RTRIM(v.AccountID)) = LTRIM(RTRIM(m.AccountID))
        ORDER BY v.SNo DESC
    ) i
    LEFT JOIN vwGroupItemsWithoutDepr g ON LTRIM(RTRIM(g.GroupID)) = LTRIM(RTRIM(m.GroupID))
)";

        var countSql = $@"{baseSql}
SELECT COUNT(*)
FROM Enriched e
WHERE {whereClause};";

        var pageSql = $@"{baseSql}
SELECT
    e.SNo,
    e.AccountNo,
    e.AccountName,
    e.GroupID,
    e.GroupName,
    e.AccountDesc,
    e.AccountOpAmt,
    e.AccountClAmt
FROM Enriched e
WHERE {whereClause}
ORDER BY {sortColumn} {sortDirection}, e.SNo DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        var parameters = new
        {
            Search = $"%{search}%",
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        };

        var totalRows = await db.LoadDataText<int, dynamic>(countSql, parameters, AcctConn);
        var items = await db.LoadDataText<ChartOfAccountListItem, dynamic>(pageSql, parameters, AcctConn);

        return new PagedChartOfAccountResult
        {
            TotalCount = totalRows.FirstOrDefault(),
            Page = page,
            PageSize = pageSize,
            Items = items.ToList()
        };
    }

    public async Task<ChartOfAccountEntry?> GetByIdAsync(long sNo, CancellationToken ct = default)
    {
        const string sql = @"
SELECT TOP 1
    m.SNo,
    LTRIM(RTRIM(m.AccountNo)) AS AccountNo,
    LTRIM(RTRIM(m.AccountName)) AS AccountName,
    LTRIM(RTRIM(m.GroupID)) AS GroupID,
    LTRIM(RTRIM(g.GroupName)) AS GroupName,
    m.AccountDesc,
    m.AccountOpAmt,
    m.AccountClAmt
FROM dbo.ChartOfAccountMaster m
LEFT JOIN vwGroupItemsWithoutDepr g ON LTRIM(RTRIM(g.GroupID)) = LTRIM(RTRIM(m.GroupID))
WHERE m.SNo = @SNo;";

        var rows = await db.LoadDataText<ChartOfAccountEntry, dynamic>(sql, new { SNo = sNo }, AcctConn);
        return rows.FirstOrDefault();
    }

    public async Task<ChartOfAccountDefaults> GetDefaultsAsync(CancellationToken ct = default)
    {
        var defaults = await emrDefaults.GetAsync(ct);
        return new ChartOfAccountDefaults
        {
            AutoAccountNo = defaults.Get("AUTO_ACCT_NO", "YES"),
            ReceiveExtData = defaults.Get("Receive_Ext_Data", "NO"),
            ReceiveArData = defaults.Get("Receive_AR_Data", "NO"),
            ReceiveApData = defaults.Get("Receive_AP_Data", "NO"),
            ReceiveExpenseData = defaults.Get("Receive_EXPENSE_Data", "NO"),
            ReceivePayrollData = defaults.Get("Receive_PAYROLL_Data", "NO")
        };
    }

    public async Task<List<ChartOfAccountGroupLookup>> GetGroupsAsync(CancellationToken ct = default)
    {
        const string sql = @"
SELECT '' AS GroupID, '' AS GroupName
UNION
SELECT DISTINCT
    LTRIM(RTRIM(GroupID)) AS GroupID,
    LTRIM(RTRIM(GroupName)) AS GroupName
FROM vwGroupItemsWithoutDepr
ORDER BY GroupName;";

        var rows = await db.LoadDataText<ChartOfAccountGroupLookup, dynamic>(sql, new { }, AcctConn);
        return rows.ToList();
    }

    public async Task<string> GetNextAccountNoAsync(string groupId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(groupId))
        {
            return string.Empty;
        }

        const string sql = "SELECT dbo.udf_getNextAcctID(@GroupID) AS AccountNo;";
        var rows = await db.LoadDataText<NextAccountNoRow, dynamic>(sql, new { GroupID = groupId.Trim() }, AcctConn);
        return rows.FirstOrDefault()?.AccountNo?.Trim() ?? string.Empty;
    }

    public async Task<ChartOfAccountEntry> CreateAsync(ChartOfAccountEntry entry, CancellationToken ct = default)
    {
        await ValidateAsync(entry, null, ct);

        var defaults = await emrDefaults.GetAsync(ct);
        var accountNo = entry.AccountNo.Trim();

        if (string.Equals(defaults.Get("AUTO_ACCT_NO", "YES"), "YES", StringComparison.OrdinalIgnoreCase))
        {
            accountNo = await GetNextAccountNoAsync(entry.GroupID, ct);
            if (accountNo.Length != 7)
            {
                throw new InvalidOperationException("Invalid Account No, Please re-select Account Group Name");
            }
        }

        const string sql = @"
INSERT INTO dbo.ChartOfAccountMaster
(
    AccountID,
    AccountNo,
    GroupID,
    AccountName,
    AccountOpAmt,
    AccountClAmt,
    AccountDesc,
    Hidden,
    isContra
)
VALUES
(
    @AccountID,
    @AccountNo,
    @GroupID,
    @AccountName,
    @AccountOpAmt,
    @AccountClAmt,
    @AccountDesc,
    0,
    0
);

SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS SNo;";

        var accountId = CreateAccountId(defaults.Get("CoyID", "0001"), accountNo);

        var rows = await db.LoadDataText<CreateIdentityRow, dynamic>(sql, new
        {
            AccountID = accountId,
            AccountNo = accountNo,
            GroupID = entry.GroupID.Trim(),
            AccountName = entry.AccountName.Trim(),
            AccountOpAmt = 0m,
            AccountClAmt = 0m,
            AccountDesc = string.IsNullOrWhiteSpace(entry.AccountDesc) ? null : entry.AccountDesc.Trim()
        }, AcctConn);

        var sNo = rows.FirstOrDefault()?.SNo ?? 0;
        if (sNo <= 0)
        {
            throw new InvalidOperationException("Unable to create account.");
        }

        logger.LogInformation("Chart of account created. SNo: {SNo}, AccountNo: {AccountNo}", sNo, accountNo);

        var created = await GetByIdAsync(sNo, ct);
        return created ?? new ChartOfAccountEntry
        {
            SNo = sNo,
            AccountNo = accountNo,
            AccountName = entry.AccountName.Trim(),
            GroupID = entry.GroupID.Trim(),
            AccountDesc = string.IsNullOrWhiteSpace(entry.AccountDesc) ? null : entry.AccountDesc.Trim(),
            AccountOpAmt = 0m,
            AccountClAmt = 0m,
        };
    }

    public async Task<ChartOfAccountEntry> UpdateAsync(ChartOfAccountEntry entry, CancellationToken ct = default)
    {
        if (entry.SNo is null || entry.SNo.Value <= 0)
        {
            throw new InvalidOperationException("Account id is required.");
        }

        var existing = await GetByIdAsync(entry.SNo.Value, ct);
        if (existing is null)
        {
            throw new InvalidOperationException("Account was not found.");
        }

        await ValidateAsync(entry, existing, ct);

        var defaults = await emrDefaults.GetAsync(ct);
        var autoAcctNo = string.Equals(defaults.Get("AUTO_ACCT_NO", "YES"), "YES", StringComparison.OrdinalIgnoreCase);

        var accountNo = existing.AccountNo;
        var groupChanged = !string.Equals(existing.GroupID.Trim(), entry.GroupID.Trim(), StringComparison.OrdinalIgnoreCase);

        if (autoAcctNo && groupChanged)
        {
            accountNo = await GetNextAccountNoAsync(entry.GroupID, ct);
            if (accountNo.Length != 7)
            {
                throw new InvalidOperationException("Invalid Account No, Please re-select Account Group Name");
            }
        }
        else if (!autoAcctNo)
        {
            accountNo = entry.AccountNo.Trim();
        }

        const string sql = @"
UPDATE dbo.ChartOfAccountMaster
SET
    AccountID = @AccountID,
    AccountNo = @AccountNo,
    GroupID = @GroupID,
    AccountName = @AccountName,
    AccountDesc = @AccountDesc,
    AccountOpAmt = @AccountOpAmt,
    AccountClAmt = @AccountClAmt
WHERE SNo = @SNo;";

        var accountId = CreateAccountId(defaults.Get("CoyID", "0001"), accountNo);

        await db.SaveDataText(sql, new
        {
            SNo = entry.SNo.Value,
            AccountID = accountId,
            AccountNo = accountNo,
            GroupID = entry.GroupID.Trim(),
            AccountName = entry.AccountName.Trim(),
            AccountDesc = string.IsNullOrWhiteSpace(entry.AccountDesc) ? null : entry.AccountDesc.Trim(),
            AccountOpAmt = existing.AccountOpAmt,
            AccountClAmt = existing.AccountClAmt
        }, AcctConn);

        logger.LogInformation("Chart of account updated. SNo: {SNo}, AccountNo: {AccountNo}", entry.SNo.Value, accountNo);

        var updated = await GetByIdAsync(entry.SNo.Value, ct);
        return updated ?? new ChartOfAccountEntry
        {
            SNo = entry.SNo.Value,
            AccountNo = accountNo,
            AccountName = entry.AccountName.Trim(),
            GroupID = entry.GroupID.Trim(),
            AccountDesc = string.IsNullOrWhiteSpace(entry.AccountDesc) ? null : entry.AccountDesc.Trim(),
            AccountOpAmt = existing.AccountOpAmt,
            AccountClAmt = existing.AccountClAmt,
        };
    }

    public async Task DeleteAsync(long sNo, CancellationToken ct = default)
    {
        const string checkSql = @"
SELECT TOP 1
    SNo,
    AccountID,
    AccountNo,
    AccountName
FROM vwChartOfAccountMasterForDelete
WHERE SNo = @SNo;";

        var rows = await db.LoadDataText<DeleteCandidateRow, dynamic>(checkSql, new { SNo = sNo }, AcctConn);
        var candidate = rows.FirstOrDefault();
        if (candidate is null)
        {
            throw new InvalidOperationException("Account cannot be deleted. It may already have transactions or does not exist.");
        }

        const string deleteSql = "DELETE FROM dbo.ChartOfAccountMaster WHERE SNo = @SNo;";
        await db.SaveDataText(deleteSql, new { SNo = sNo }, AcctConn);

        logger.LogInformation("Chart of account deleted. SNo: {SNo}, AccountNo: {AccountNo}", sNo, candidate.AccountNo);
    }

    private async Task ValidateAsync(ChartOfAccountEntry entry, ChartOfAccountEntry? existing, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(entry.GroupID))
        {
            throw new InvalidOperationException("Please enter Account Group Name");
        }

        if (string.IsNullOrWhiteSpace(entry.AccountName))
        {
            throw new InvalidOperationException("Please enter Account Name");
        }

        var normalizedGroupId = entry.GroupID.Trim();
        if ((existing is null || !string.Equals(existing.GroupID.Trim(), normalizedGroupId, StringComparison.OrdinalIgnoreCase))
            && normalizedGroupId.StartsWith("11", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Cannot Create Fixed Assets here, Use the Fixed Assets Module");
        }

        var groupExists = (await db.LoadDataText<int, dynamic>(
            "SELECT COUNT(1) FROM vwGroupItemsWithoutDepr WHERE GroupID = @GroupID",
            new { GroupID = normalizedGroupId },
            AcctConn)).FirstOrDefault() > 0;

        if (!groupExists)
        {
            throw new InvalidOperationException("Selected account group was not found.");
        }

        var defaults = await emrDefaults.GetAsync(ct);
        var autoAcctNo = string.Equals(defaults.Get("AUTO_ACCT_NO", "YES"), "YES", StringComparison.OrdinalIgnoreCase);

        if (!autoAcctNo)
        {
            if (string.IsNullOrWhiteSpace(entry.AccountNo))
            {
                throw new InvalidOperationException("Account No is required.");
            }

            var accountNo = entry.AccountNo.Trim();
            var duplicateCount = (await db.LoadDataText<int, dynamic>(
                "SELECT COUNT(1) FROM dbo.ChartOfAccountMaster WHERE AccountNo = @AccountNo AND (@SNo IS NULL OR SNo <> @SNo)",
                new { AccountNo = accountNo, SNo = entry.SNo },
                AcctConn)).FirstOrDefault();

            if (duplicateCount > 0)
            {
                throw new InvalidOperationException("Account No already exists.");
            }
        }
    }

    private static string CreateAccountId(string coyId, string accountNo)
    {
        return $"{coyId?.Trim() ?? "0001"}{accountNo?.Trim()}";
    }

    private sealed class NextAccountNoRow
    {
        public string AccountNo { get; set; } = string.Empty;
    }

    private sealed class CreateIdentityRow
    {
        public long SNo { get; set; }
    }

    private sealed class DeleteCandidateRow
    {
        public long SNo { get; set; }
        public string AccountID { get; set; } = string.Empty;
        public string AccountNo { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
    }
}
