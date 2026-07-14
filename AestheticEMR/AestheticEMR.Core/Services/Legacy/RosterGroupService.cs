using AestheticEMR.Core.Services.Account;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Legacy;

public class RosterGroupService(
    IConfiguration configuration,
    IUserIdAccessor userIdAccessor,
    IEmrAppDefaultsService defaultsService,
    ILogger<RosterGroupService> logger) : IRosterGroupService
{
    private const string ConnectionName = "smartHRConnection";
    private const string IdentityConnectionName = "DefaultConnection";

    public async Task<string> GetCurrentDepartmentNameAsync(CancellationToken cancellationToken = default)
    {
        var deptId = await ResolveDepartmentIdAsync(cancellationToken);
        await using var connection = await OpenConnectionAsync(cancellationToken);
        const string sql = @"SELECT TOP 1 LTRIM(RTRIM(DeptName)) AS Value FROM empDepartments WHERE DeptID = @DeptId;";
        var row = await connection.QueryFirstOrDefaultAsync<NameRow>(sql, new { DeptId = deptId });
        return row?.Value ?? string.Empty;
    }

    public async Task<IEnumerable<RosterGroupDepartmentItem>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        const string sql = @"
SELECT DeptId, DeptName
FROM (
    SELECT '__ALL__' AS DeptId, '(ALL)' AS DeptName, 0 AS SortOrder
    UNION ALL
    SELECT
        LTRIM(RTRIM(DeptID)) AS DeptId,
        LTRIM(RTRIM(DeptName)) AS DeptName,
        1 AS SortOrder
    FROM empDepartments
    WHERE LTRIM(RTRIM(ISNULL(DeptID, ''))) <> ''
) d
ORDER BY d.SortOrder, d.DeptName;";

        var rows = await connection.QueryAsync<RosterGroupDepartmentItem>(sql);
        return rows.ToList();
    }

    public async Task<IEnumerable<RosterGroupGridItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        const string sql = @"
select  GroupName,StaffName,DeptName,Assigned ,GroupID,EmpID   from qryEmployeesAssignedToRosterGroupAll";

        var rows = await connection.QueryAsync<RosterGroupGridItem>(sql);
        return rows.ToList();
    }

    public async Task<IEnumerable<RosterGroupAvailableStaffItem>> GetAvailableStaffAsync(string? deptId = null, CancellationToken cancellationToken = default)
    {
        var selectedDeptId = NormalizeText(deptId);
        if (string.IsNullOrWhiteSpace(selectedDeptId))
        {
            selectedDeptId = await ResolveDepartmentIdAsync(cancellationToken);
        }

        await using var connection = await OpenConnectionAsync(cancellationToken);

        const string sqlForAll = @"
select Distinct EmpID,StaffName,DeptID from qryEmployeesNotAssignedToRosterGroupYet order by StaffName";

        const string sqlForDept = @"
select Distinct EmpID,StaffName,DeptID from qryEmployeesNotAssignedToRosterGroupYet where deptID=@DeptID order by StaffName";

        var rows = string.Equals(selectedDeptId, "__ALL__", StringComparison.OrdinalIgnoreCase)
            ? await connection.QueryAsync<AvailableStaffRow>(sqlForAll)
            : await connection.QueryAsync<AvailableStaffRow>(sqlForDept, new { DeptID = selectedDeptId });

        return rows
            .Select(x => new RosterGroupAvailableStaffItem
            {
                EmpId = x.EmpID?.Trim() ?? string.Empty,
                FullName = x.StaffName?.Trim() ?? string.Empty,
                DeptId = x.DeptID?.Trim()
            })
            .ToList();
    }

    public async Task<RosterGroupItem?> GetByIdAsync(long rosterGrpId, CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        const string sql = @"
SELECT TOP 1
    CAST(rg.RosterGrpID AS bigint) AS RosterGrpId,
    LTRIM(RTRIM(rg.RosterGrpName)) AS RosterGrpName,
    LTRIM(RTRIM(rg.DeptID)) AS DeptId,
    LTRIM(RTRIM(ISNULL(d.DeptName, ''))) AS DeptName,
    LTRIM(RTRIM(ISNULL(rg.Exempted, 'NO'))) AS Exempted,
    COUNT(e.EmpID) AS EmployeeCount
FROM RosterGroup rg
LEFT JOIN empDepartments d ON d.DeptID = rg.DeptID
LEFT JOIN Employees e ON e.RosterGrpID = rg.RosterGrpID
WHERE rg.RosterGrpID = @RosterGrpId
GROUP BY rg.RosterGrpID, rg.RosterGrpName, rg.DeptID, d.DeptName, rg.Exempted;";

        return await connection.QueryFirstOrDefaultAsync<RosterGroupItem>(sql, new { RosterGrpId = rosterGrpId });
    }

    public async Task<RosterGroupItem> CreateAsync(RosterGroupSaveRequest request, string currentUserName, CancellationToken cancellationToken = default)
    {
        var staffIds = NormalizeEmpIds(request.EmpIds);
        if (staffIds.Count == 0)
        {
            throw new InvalidOperationException("At least one employee is required.");
        }

        var deptId = NormalizeText(request.DeptId) ?? await ResolveDepartmentIdAsync(cancellationToken);
        var groupName = NormalizeText(request.RosterGrpName) ?? "Roster Group";
        await using var connection = await OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var rosterGrpId = await connection.ExecuteScalarAsync<long>(@"
INSERT INTO RosterGroup (RosterGrpName, DeptID, Exempted)
OUTPUT INSERTED.RosterGrpID
VALUES (@RosterGrpName, @DeptID, @Exempted);",
                new
                {
                    RosterGrpName = groupName,
                    DeptID = deptId,
                    Exempted = NormalizeText(request.Exempted) ?? "NO"
                }, transaction);

            await connection.ExecuteAsync(@"UPDATE Employees SET RosterGrpID = @RosterGrpId WHERE EmpID = @EmpId;",
                staffIds.Select(empId => new { RosterGrpId = rosterGrpId, EmpId = empId }), transaction);

            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Created roster group {RosterGrpId} by {User}", rosterGrpId, currentUserName);
            return (await GetByIdAsync(rosterGrpId, cancellationToken))!;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<RosterGroupItem> UpdateAsync(long rosterGrpId, RosterGroupSaveRequest request, string currentUserName, CancellationToken cancellationToken = default)
    {
        var staffIds = NormalizeEmpIds(request.EmpIds);
        if (staffIds.Count == 0)
        {
            throw new InvalidOperationException("At least one employee is required.");
        }

        var deptId = NormalizeText(request.DeptId) ?? await ResolveDepartmentIdAsync(cancellationToken);
        var groupName = NormalizeText(request.RosterGrpName) ?? "Roster Group";
        await using var connection = await OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            await connection.ExecuteAsync(@"UPDATE RosterGroup SET RosterGrpName = @RosterGrpName, DeptID = @DeptId, Exempted = @Exempted WHERE RosterGrpID = @RosterGrpId;",
                new { RosterGrpId = rosterGrpId, RosterGrpName = groupName, DeptId = deptId, Exempted = NormalizeText(request.Exempted) ?? "NO" }, transaction);
            await connection.ExecuteAsync(@"UPDATE Employees SET RosterGrpID = 0 WHERE RosterGrpID = @RosterGrpId;", new { RosterGrpId = rosterGrpId }, transaction);
            await connection.ExecuteAsync(@"UPDATE Employees SET RosterGrpID = @RosterGrpId WHERE EmpID = @EmpId;",
                staffIds.Select(empId => new { RosterGrpId = rosterGrpId, EmpId = empId }), transaction);

            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Updated roster group {RosterGrpId} by {User}", rosterGrpId, currentUserName);
            return (await GetByIdAsync(rosterGrpId, cancellationToken))!;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long rosterGrpId, string currentUserName, CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            await connection.ExecuteAsync(@"UPDATE Employees SET RosterGrpID = 0 WHERE RosterGrpID = @RosterGrpId;", new { RosterGrpId = rosterGrpId }, transaction);
            var affected = await connection.ExecuteAsync(@"DELETE FROM RosterGroup WHERE RosterGrpID = @RosterGrpId;", new { RosterGrpId = rosterGrpId }, transaction);
            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Deleted roster group {RosterGrpId} by {User}", rosterGrpId, currentUserName);
            return affected > 0;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<string> ResolveDepartmentIdAsync(CancellationToken cancellationToken)
    {
        var empId = userIdAccessor.GetCurrentUserEmpId();
        if (string.IsNullOrWhiteSpace(empId))
        {
            throw new InvalidOperationException("Unable to resolve the current department.");
        }

        await using var connection = await OpenConnectionAsync(cancellationToken);
        const string sql = @"SELECT TOP 1 LTRIM(RTRIM(DeptID)) AS Value FROM qryEmp WHERE EmpID = @EmpId;";
        var row = await connection.QueryFirstOrDefaultAsync<NameRow>(sql, new { EmpId = empId.Trim() });
        if (string.IsNullOrWhiteSpace(row?.Value))
        {
            throw new InvalidOperationException("Unable to resolve the current department.");
        }

        return row.Value.Trim();
    }

    private async Task<bool> IsUserInManagementRoleAsync(CancellationToken cancellationToken)
    {
        var userId = userIdAccessor.GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        await using var connection = await OpenConnectionAsync(IdentityConnectionName, cancellationToken);
        const string sql = @"
SELECT CAST(CASE WHEN EXISTS (
    SELECT 1
    FROM AspNetUserRoles ur
    INNER JOIN AspNetRoles r ON r.Id = ur.RoleId
    WHERE ur.UserId = @UserId
      AND LOWER(LTRIM(RTRIM(ISNULL(r.Name, '')))) IN ('management', 'admin')
) THEN 1 ELSE 0 END AS bit);";

        return await connection.ExecuteScalarAsync<bool>(sql, new { UserId = userId.Trim() });
    }

    private async Task<SqlConnection> OpenConnectionAsync(CancellationToken cancellationToken)
    {
        return await OpenConnectionAsync(ConnectionName, cancellationToken);
    }

    private async Task<SqlConnection> OpenConnectionAsync(string connectionName, CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString(connectionName)
            ?? throw new InvalidOperationException($"Connection string '{connectionName}' was not found.");

        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

    private static List<string> NormalizeEmpIds(IEnumerable<string> empIds)
    {
        return empIds
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string? NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private sealed class NameRow
    {
        public string? Value { get; set; }
    }

    private sealed class AvailableStaffRow
    {
        public string? EmpID { get; set; }
        public string? StaffName { get; set; }
        public string? DeptID { get; set; }
    }
}
