using AestheticEMR.Core.Models.Employees;
using AestheticEMR.Core.Services.Employees.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AestheticEMR.Core.Services.Employees;

public class DepartmentService(
    IConfiguration configuration,
    ILogger<DepartmentService> logger) : IDepartmentService
{
    private const string ConnectionName = "smartHRConnection";

    // Legacy format: zero-padded 2 chars (e.g. "01", "10", "99").
    private const int MaxDepartmentId = 99;
    private const string IdFormat = "00";

    public async Task<string> GenerateDepartmentIdAsync()
    {
        await using var connection = await OpenConnectionAsync();

        var currentMax = await connection.ExecuteScalarAsync<int>(@"
SELECT ISNULL(MAX(
    CASE
        WHEN LTRIM(RTRIM(ISNULL(DeptID, ''))) <> ''
             AND LTRIM(RTRIM(DeptID)) NOT LIKE '%[^0-9]%'
        THEN CAST(LTRIM(RTRIM(DeptID)) AS int)
        ELSE 0
    END), 0)
FROM EmpDepartments;");

        var nextId = currentMax + 1;
        if (nextId > MaxDepartmentId)
            throw new InvalidOperationException(
                $"Department id limit reached ({MaxDepartmentId}). Cannot generate a new department.");

        return nextId.ToString(IdFormat);
    }

    public async Task<IEnumerable<EmpDepartments>> GetAllAsync()
    {
        await using var connection = await OpenConnectionAsync();
        const string sql = @"
SELECT
    LTRIM(RTRIM(DeptID)) AS DeptId,
    LTRIM(RTRIM(ISNULL(DeptName, ''))) AS DeptName,
    LTRIM(RTRIM(ISNULL(DeptAddress, ''))) AS DeptAddress,
    LTRIM(RTRIM(ISNULL(Location, ''))) AS Location
FROM EmpDepartments
ORDER BY DeptID;";

        var rows = await connection.QueryAsync<EmpDepartments>(sql);
        return rows.ToList();
    }

    public async Task<EmpDepartments?> GetByIdAsync(string deptId)
    {
        var normalizedId = NormalizeText(deptId);
        if (string.IsNullOrWhiteSpace(normalizedId))
            return null;

        await using var connection = await OpenConnectionAsync();
        const string sql = @"
SELECT TOP 1
    LTRIM(RTRIM(DeptID)) AS DeptId,
    LTRIM(RTRIM(ISNULL(DeptName, ''))) AS DeptName,
    LTRIM(RTRIM(ISNULL(DeptAddress, ''))) AS DeptAddress,
    LTRIM(RTRIM(ISNULL(Location, ''))) AS Location
FROM EmpDepartments
WHERE LTRIM(RTRIM(DeptID)) = @DeptId;";

        return await connection.QueryFirstOrDefaultAsync<EmpDepartments>(sql, new { DeptId = normalizedId });
    }

    public async Task<EmpDepartments> CreateAsync(EmpDepartments department)
    {
        await using var connection = await OpenConnectionAsync();
        await using var transaction = await connection.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            var currentMax = await connection.ExecuteScalarAsync<int>(@"
SELECT ISNULL(MAX(
    CASE
        WHEN LTRIM(RTRIM(ISNULL(DeptID, ''))) <> ''
             AND LTRIM(RTRIM(DeptID)) NOT LIKE '%[^0-9]%'
        THEN CAST(LTRIM(RTRIM(DeptID)) AS int)
        ELSE 0
    END), 0)
FROM EmpDepartments WITH (UPDLOCK, HOLDLOCK);", transaction: transaction);

            var nextId = currentMax + 1;
            if (nextId > MaxDepartmentId)
                throw new InvalidOperationException(
                    $"Department id limit reached ({MaxDepartmentId}). Cannot generate a new department.");

            department.DeptId = nextId.ToString(IdFormat);
            department.DeptName = NormalizeText(department.DeptName) ?? string.Empty;
            department.DeptAddress = NormalizeText(department.DeptAddress);
            department.Location = NormalizeText(department.Location);

            const string insertSql = @"
INSERT INTO EmpDepartments (DeptID, DeptName, DeptAddress, Location)
VALUES (@DeptId, @DeptName, @DeptAddress, @Location);";

            await connection.ExecuteAsync(insertSql, new
            {
                DeptId = department.DeptId,
                DeptName = department.DeptName,
                DeptAddress = department.DeptAddress,
                Location = department.Location
            }, transaction);

            await transaction.CommitAsync();
            logger.LogInformation("Created department {DeptId}", department.DeptId);
            return department;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<EmpDepartments> UpdateAsync(EmpDepartments department)
    {
        var normalizedId = NormalizeText(department.DeptId)
            ?? throw new KeyNotFoundException("Department id is required.");

        await using var connection = await OpenConnectionAsync();
        const string updateSql = @"
UPDATE EmpDepartments
SET DeptName = @DeptName,
    DeptAddress = @DeptAddress,
    Location = @Location
WHERE LTRIM(RTRIM(DeptID)) = @DeptId;";

        var affected = await connection.ExecuteAsync(updateSql, new
        {
            DeptId = normalizedId,
            DeptName = NormalizeText(department.DeptName) ?? string.Empty,
            DeptAddress = NormalizeText(department.DeptAddress),
            Location = NormalizeText(department.Location)
        });

        if (affected == 0)
            throw new KeyNotFoundException($"Department {normalizedId} not found.");

        var refreshed = await GetByIdAsync(normalizedId);
        logger.LogInformation("Updated department {DeptId}", normalizedId);
        return refreshed ?? department;
    }

    public async Task<bool> DeleteAsync(string deptId)
    {
        var normalizedId = NormalizeText(deptId);
        if (string.IsNullOrWhiteSpace(normalizedId))
            return false;

        if (await IsInUseAsync(normalizedId))
            throw new InvalidOperationException(
                $"Department '{normalizedId}' is currently assigned to one or more employees and cannot be deleted.");

        await using var connection = await OpenConnectionAsync();
        const string sql = @"DELETE FROM EmpDepartments WHERE LTRIM(RTRIM(DeptID)) = @DeptId;";

        var affected = await connection.ExecuteAsync(sql, new { DeptId = normalizedId });
        if (affected > 0)
            logger.LogInformation("Deleted department {DeptId}", normalizedId);

        return affected > 0;
    }

    public async Task<bool> IsInUseAsync(string deptId)
    {
        var normalizedId = NormalizeText(deptId);
        if (string.IsNullOrWhiteSpace(normalizedId))
            return false;

        await using var connection = await OpenConnectionAsync();
        const string sql = @"
SELECT COUNT(1)
FROM HrEmployees
WHERE LTRIM(RTRIM(ISNULL(DeptID, ''))) = @DeptId;";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { DeptId = normalizedId });
        return count > 0;
    }

    public async Task<IReadOnlyDictionary<string, int>> GetInUseCountsAsync()
    {
        await using var connection = await OpenConnectionAsync();
        const string sql = @"
SELECT
    LTRIM(RTRIM(DeptID)) AS DeptId,
    COUNT(1) AS [Count]
FROM HrEmployees
WHERE LTRIM(RTRIM(ISNULL(DeptID, ''))) <> ''
GROUP BY LTRIM(RTRIM(DeptID));";

        var rows = await connection.QueryAsync<DepartmentUsageCount>(sql);
        return rows
            .Where(x => !string.IsNullOrWhiteSpace(x.DeptId))
            .ToDictionary(x => x.DeptId, x => x.Count);
    }

    private async Task<SqlConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = configuration.GetConnectionString(ConnectionName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionName}' was not found.");

        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

    private static string? NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private class DepartmentUsageCount
    {
        public string DeptId { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
