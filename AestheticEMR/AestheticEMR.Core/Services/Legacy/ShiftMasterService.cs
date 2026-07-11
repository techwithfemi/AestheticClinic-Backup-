using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AestheticEMR.Core.Services.Legacy;

public class ShiftMasterService(
    IConfiguration configuration,
    ILogger<ShiftMasterService> logger) : IShiftMasterService
{
    private const string ConnectionName = "smartHRConnection";

    public async Task<IEnumerable<ShiftMasterItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        const string sql = @"
SELECT
    CAST(s.SNo AS bigint) AS ShiftId,
    LTRIM(RTRIM(s.ShiftName)) AS ShiftName,
    COUNT(ds.DeptID) AS DepartmentCount
FROM EmpAttendanceShift s
LEFT JOIN EmpDeptShifts ds ON ds.ShiftID = s.SNo
GROUP BY s.SNo, s.ShiftName
ORDER BY s.ShiftName;";

        var rows = await connection.QueryAsync<ShiftMasterItem>(sql);
        return rows.ToList();
    }

    public async Task<IEnumerable<DepartmentLookupItem>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        const string sql = @"
SELECT
    LTRIM(RTRIM(DeptId)) AS DeptId,
    LTRIM(RTRIM(ISNULL(DeptName, ''))) AS DeptName,
    LTRIM(RTRIM(ISNULL(Location, ''))) AS Location
FROM EmpDepartments
ORDER BY DeptName;";

        var rows = await connection.QueryAsync<DepartmentLookupItem>(sql);
        return rows.ToList();
    }

    public async Task<ShiftMasterDetail?> GetByIdAsync(long shiftId, CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        const string shiftSql = @"
SELECT TOP 1
    CAST(SNo AS bigint) AS ShiftId,
    LTRIM(RTRIM(ShiftName)) AS ShiftName
FROM EmpAttendanceShift
WHERE SNo = @ShiftId;";

        var shift = await connection.QueryFirstOrDefaultAsync<ShiftMasterDetail>(shiftSql, new { ShiftId = shiftId });
        if (shift is null)
        {
            return null;
        }

        const string deptSql = @"
SELECT DISTINCT LTRIM(RTRIM(DeptID)) AS DeptId
FROM EmpDeptShifts
WHERE ShiftID = @ShiftId
ORDER BY DeptID;";

        var deptIds = await connection.QueryAsync<string>(deptSql, new { ShiftId = shiftId });
        shift.DeptIds = deptIds.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();
        return shift;
    }

    public async Task<ShiftMasterDetail> CreateAsync(ShiftMasterSaveRequest request, string currentUserName, CancellationToken cancellationToken = default)
    {
        var shiftName = NormalizeText(request.ShiftName);
        if (string.IsNullOrWhiteSpace(shiftName))
        {
            throw new InvalidOperationException("Shift name is required.");
        }

        var deptIds = NormalizeDeptIds(request.DeptIds);
        if (deptIds.Count == 0)
        {
            throw new InvalidOperationException("Select at least one department.");
        }

        await using var connection = await OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var shiftId = await connection.ExecuteScalarAsync<long>(@"
INSERT INTO EmpAttendanceShift (ShiftName)
OUTPUT INSERTED.SNo
VALUES (@ShiftName);",
                new { ShiftName = shiftName }, transaction);

            await InsertAssignmentsAsync(connection, transaction, shiftId, deptIds);
            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Created shift master {ShiftId} by {User}", shiftId, currentUserName);
            return (await GetByIdAsync(shiftId, cancellationToken))!;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<ShiftMasterDetail> UpdateAsync(long shiftId, ShiftMasterSaveRequest request, string currentUserName, CancellationToken cancellationToken = default)
    {
        var shiftName = NormalizeText(request.ShiftName);
        if (string.IsNullOrWhiteSpace(shiftName))
        {
            throw new InvalidOperationException("Shift name is required.");
        }

        var deptIds = NormalizeDeptIds(request.DeptIds);
        if (deptIds.Count == 0)
        {
            throw new InvalidOperationException("Select at least one department.");
        }

        await using var connection = await OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var affected = await connection.ExecuteAsync(@"
UPDATE EmpAttendanceShift
SET ShiftName = @ShiftName
WHERE SNo = @ShiftId;",
                new { ShiftId = shiftId, ShiftName = shiftName }, transaction);

            if (affected == 0)
            {
                throw new KeyNotFoundException($"Shift {shiftId} not found.");
            }

            await connection.ExecuteAsync(@"DELETE FROM EmpDeptShifts WHERE ShiftID = @ShiftId;", new { ShiftId = shiftId }, transaction);
            await InsertAssignmentsAsync(connection, transaction, shiftId, deptIds);
            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Updated shift master {ShiftId} by {User}", shiftId, currentUserName);
            return (await GetByIdAsync(shiftId, cancellationToken))!;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long shiftId, string currentUserName, CancellationToken cancellationToken = default)
    {
        await using var connection = await OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        try
        {
            var existing = await connection.QueryFirstOrDefaultAsync<ShiftMasterDetail>(@"SELECT TOP 1 CAST(SNo AS bigint) AS ShiftId, LTRIM(RTRIM(ShiftName)) AS ShiftName FROM EmpAttendanceShift WHERE SNo = @ShiftId;", new { ShiftId = shiftId }, transaction);
            if (existing is null)
            {
                return false;
            }

            if (existing.ShiftName.Trim().Equals("(OFF_DUTY)", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("(OFF_DUTY) Shift Cannot be Deleted.");
            }

            await connection.ExecuteAsync(@"DELETE FROM EmpDeptShifts WHERE ShiftID = @ShiftId;", new { ShiftId = shiftId }, transaction);
            var affected = await connection.ExecuteAsync(@"DELETE FROM EmpAttendanceShift WHERE SNo = @ShiftId;", new { ShiftId = shiftId }, transaction);
            await transaction.CommitAsync(cancellationToken);
            logger.LogInformation("Deleted shift master {ShiftId} by {User}", shiftId, currentUserName);
            return affected > 0;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private static List<string> NormalizeDeptIds(IEnumerable<string> deptIds)
    {
        return deptIds
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static string? NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private async Task<SqlConnection> OpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString(ConnectionName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionName}' was not found.");

        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }

    private static async Task InsertAssignmentsAsync(SqlConnection connection, IDbTransaction transaction, long shiftId, IEnumerable<string> deptIds)
    {
        foreach (var deptId in deptIds)
        {
            await connection.ExecuteAsync(@"INSERT INTO EmpDeptShifts (ShiftID, DeptID) VALUES (@ShiftId, @DeptId);",
                new { ShiftId = shiftId, DeptId = deptId }, transaction);
        }
    }
}
