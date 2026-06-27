using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Employees;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Employees.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmployeeEntity = AestheticEMR.Core.Models.Employees.Employees;

namespace AestheticEMR.Core.Services.Employees;

public class EmployeeService(ApplicationDbContext context, ILogger<EmployeeService> logger) : IEmployeeService
{
    private const string EmpIdCode = "Employee";
    private const string EmpIdPrefix = "HR-";

    public async Task<string> GenerateEmpIdAsync()
    {
        var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == EmpIdCode);
        var nextId = (idgen?.Id ?? 0) + 1;
        return $"{EmpIdPrefix}{Convert.ToInt64(nextId):D7}";
    }

    public async Task<IEnumerable<EmployeeEntity>> GetAllAsync()
    {
        return await context.HrEmployees
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<EmployeeEntity?> GetByIdAsync(string empId)
    {
        return await context.HrEmployees.FirstOrDefaultAsync(e => e.EmpId == empId);
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
                context.HrIdgens.Update(idgen);
            }

            employee.EmpId = $"{EmpIdPrefix}{Convert.ToInt64(nextId):D7}";
            context.HrEmployees.Add(employee);
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
        // Bulletproof update: only touch the columns we own, regardless of how
        // the entity was constructed. Avoids EF tracking edge-cases on legacy rows
        // (different ID formats, unrelated columns, etc.).
        var rows = await context.HrEmployees
            .Where(e => e.EmpId == employee.EmpId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e.FirstName, employee.FirstName)
                .SetProperty(e => e.LastName, employee.LastName)
                .SetProperty(e => e.DeptId, employee.DeptId)
                .SetProperty(e => e.Sex, employee.Sex)
                .SetProperty(e => e.Dob, employee.Dob)
                .SetProperty(e => e.EmpStatus, employee.EmpStatus));

        if (rows == 0)
            throw new KeyNotFoundException($"Employee {employee.EmpId} not found.");

        // Designation is mapped via the entity's "Designation" column; only overwrite
        // when a non-empty value is supplied so we don't wipe legacy rows on partial VMs.
        if (!string.IsNullOrWhiteSpace(employee.Designation))
        {
            await context.HrEmployees
                .Where(e => e.EmpId == employee.EmpId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => e.Designation, employee.Designation));
        }

        var refreshed = await context.HrEmployees.AsNoTracking()
            .FirstOrDefaultAsync(e => e.EmpId == employee.EmpId);

        logger.LogInformation("Updated employee {EmpId}", employee.EmpId);
        return refreshed ?? employee;
    }

    public async Task DeleteAsync(string empId)
    {
        var existing = await context.HrEmployees.FirstOrDefaultAsync(e => e.EmpId == empId)
            ?? throw new KeyNotFoundException($"Employee {empId} not found.");

        context.HrEmployees.Remove(existing);
        await context.SaveChangesAsync();
        logger.LogInformation("Deleted employee {EmpId}", empId);
    }

    public async Task<IEnumerable<Designation>> GetDesignationsAsync()
    {
        return await context.Designations
            .AsNoTracking()
            .OrderBy(d => d.desName)
            .ToListAsync();
    }

    public async Task<IEnumerable<EmpDepartments>> GetDepartmentsAsync()
    {
        return await context.EmpDepartments
            .AsNoTracking()
            .OrderBy(d => d.DeptName)
            .ToListAsync();
    }
}
