using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Employees;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Employees.Interfaces;
using DataAccess.DbAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmployeeEntity = AestheticEMR.Core.Models.Employees.Employees;

namespace AestheticEMR.Core.Services.Employees;

public class EmployeeService(
    ApplicationDbContext context,
    ISqlDataAccess db,
    ILogger<EmployeeService> logger) : IEmployeeService
{
    private const string EmpIdCode = "Employee";
    private const string EmpIdPrefix = "HR-";
    private const string ConnectionId = "smartHRConnection";

    public async Task<string> GenerateEmpIdAsync()
    {
        var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == EmpIdCode);
        var nextId = (idgen?.Id ?? 0) + 1;
        return $"{EmpIdPrefix}{Convert.ToInt64(nextId):D7}";
    }

    public async Task<IEnumerable<EmployeeEntity>> GetAllAsync()
    {
        const string sql = @"
SELECT
    LTRIM(RTRIM(EmpID)) AS EmpId,
    LTRIM(RTRIM(ISNULL(FirstName, ''))) AS FirstName,
    LTRIM(RTRIM(ISNULL(LastName, ''))) AS LastName,
    NULLIF(LTRIM(RTRIM(ISNULL(DeptID, ''))), '') AS DeptId,
    NULLIF(LTRIM(RTRIM(ISNULL(Designation, ''))), '') AS Designation,
    NULLIF(LTRIM(RTRIM(ISNULL(EmpStatus, ''))), '') AS EmpStatus,
    Dob,
    NULLIF(LTRIM(RTRIM(ISNULL(Sex, ''))), '') AS Sex
FROM Employees
ORDER BY LastName, FirstName;";

        var rows = await db.LoadDataText<EmployeeEntity, dynamic>(sql, new { }, ConnectionId);
        return rows.ToList();
    }

    public async Task<EmployeeEntity?> GetByIdAsync(string empId)
    {
        var normalizedId = NormalizeText(empId);
        if (string.IsNullOrWhiteSpace(normalizedId))
            return null;

        const string sql = @"
SELECT TOP 1
    LTRIM(RTRIM(EmpID)) AS EmpId,
    LTRIM(RTRIM(ISNULL(FirstName, ''))) AS FirstName,
    LTRIM(RTRIM(ISNULL(LastName, ''))) AS LastName,
    NULLIF(LTRIM(RTRIM(ISNULL(DeptID, ''))), '') AS DeptId,
    NULLIF(LTRIM(RTRIM(ISNULL(Designation, ''))), '') AS Designation,
    NULLIF(LTRIM(RTRIM(ISNULL(EmpStatus, ''))), '') AS EmpStatus,
    Dob,
    NULLIF(LTRIM(RTRIM(ISNULL(Sex, ''))), '') AS Sex
FROM Employees
WHERE LTRIM(RTRIM(EmpID)) = @EmpId;";

        var rows = await db.LoadDataText<EmployeeEntity, dynamic>(sql, new { EmpId = normalizedId }, ConnectionId);
        return rows.FirstOrDefault();
    }

    public async Task<EmployeeEntity> CreateAsync(EmployeeEntity employee)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == EmpIdCode);

            decimal nextId;
            if (idgen == null)
            {
                idgen = new Idgen { DestName = EmpIdCode, Id = 1 };
                context.HrIdgens.Add(idgen);
                nextId = 1;
            }
            else
            {
                nextId = idgen.Id + 1;
                idgen.Id = nextId;
            }

            employee.EmpId = $"{EmpIdPrefix}{Convert.ToInt64(nextId):D7}";

            const string insertSql = @"
INSERT INTO Employees (EmpID, LastName, FirstName, Designation, DeptID, EmpStatus, Dob, Sex)
VALUES (@EmpId, @LastName, @FirstName, @Designation, @DeptId, @EmpStatus, @Dob, @Sex);";

            await db.SaveDataText(insertSql, new
            {
                EmpId = employee.EmpId,
                LastName = NormalizeRequired(employee.LastName),
                FirstName = NormalizeRequired(employee.FirstName),
                Designation = NormalizeText(employee.Designation),
                DeptId = NormalizeText(employee.DeptId),
                EmpStatus = NormalizeText(employee.EmpStatus) ?? "ACTIVE",
                employee.Dob,
                Sex = NormalizeText(employee.Sex)
            }, ConnectionId);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("Created employee {EmpId}", employee.EmpId);
            return employee;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<EmployeeEntity> UpdateAsync(EmployeeEntity employee)
    {
        var normalizedId = NormalizeText(employee.EmpId)
            ?? throw new KeyNotFoundException("Employee id is required.");

        const string updateSql = @"
UPDATE Employees
SET LastName = @LastName,
    FirstName = @FirstName,
    DeptID = @DeptId,
    Designation = CASE WHEN @HasDesignation = 1 THEN @Designation ELSE Designation END,
    EmpStatus = @EmpStatus,
    Dob = @Dob,
    Sex = @Sex
WHERE LTRIM(RTRIM(EmpID)) = @EmpId;";

        await db.SaveDataText(updateSql, new
        {
            EmpId = normalizedId,
            LastName = NormalizeRequired(employee.LastName),
            FirstName = NormalizeRequired(employee.FirstName),
            DeptId = NormalizeText(employee.DeptId),
            Designation = NormalizeText(employee.Designation),
            HasDesignation = !string.IsNullOrWhiteSpace(employee.Designation),
            EmpStatus = NormalizeText(employee.EmpStatus) ?? "INACTIVE",
            employee.Dob,
            Sex = NormalizeText(employee.Sex)
        }, ConnectionId);

        var refreshed = await GetByIdAsync(normalizedId)
            ?? throw new KeyNotFoundException($"Employee {normalizedId} not found.");

        logger.LogInformation("Updated employee {EmpId}", normalizedId);
        return refreshed;
    }

    public async Task DeleteAsync(string empId)
    {
        var normalizedId = NormalizeText(empId)
            ?? throw new KeyNotFoundException("Employee id is required.");

        var existing = await GetByIdAsync(normalizedId)
            ?? throw new KeyNotFoundException($"Employee {normalizedId} not found.");

        const string sql = "DELETE FROM Employees WHERE LTRIM(RTRIM(EmpID)) = @EmpId;";
        await db.SaveDataText(sql, new { EmpId = normalizedId }, ConnectionId);

        logger.LogInformation("Deleted employee {EmpId}", existing.EmpId);
    }

    public async Task<IEnumerable<Designation>> GetDesignationsAsync()
    {
        const string sql = @"
SELECT
    LTRIM(RTRIM(desID)) AS desID,
    LTRIM(RTRIM(ISNULL(desName, ''))) AS desName
FROM Designation
ORDER BY desName;";

        var rows = await db.LoadDataText<Designation, dynamic>(sql, new { }, ConnectionId);
        return rows.ToList();
    }

    public async Task<IEnumerable<EmpDepartments>> GetDepartmentsAsync()
    {
        const string sql = @"
SELECT
    LTRIM(RTRIM(DeptID)) AS DeptId,
    LTRIM(RTRIM(ISNULL(DeptName, ''))) AS DeptName,
    NULLIF(LTRIM(RTRIM(ISNULL(DeptAddress, ''))), '') AS DeptAddress,
    NULLIF(LTRIM(RTRIM(ISNULL(Location, ''))), '') AS Location
FROM EmpDepartments
ORDER BY DeptName;";

        var rows = await db.LoadDataText<EmpDepartments, dynamic>(sql, new { }, ConnectionId);
        return rows.ToList();
    }

    private static string? NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static string NormalizeRequired(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }
}
