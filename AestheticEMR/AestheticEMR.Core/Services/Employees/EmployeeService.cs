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
        var existing = await context.HrEmployees.FirstOrDefaultAsync(e => e.EmpId == employee.EmpId)
            ?? throw new KeyNotFoundException($"Employee {employee.EmpId} not found.");

        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.DeptId = employee.DeptId;
        // Only overwrite Designation when the incoming value is non-empty;
        // protects legacy records whose VM didn't round-trip the FK.
        if (!string.IsNullOrWhiteSpace(employee.Designation))
            existing.Designation = employee.Designation;
        existing.EmpStatus = employee.EmpStatus;
        existing.Dob = employee.Dob;
        existing.Sex = employee.Sex;

        await context.SaveChangesAsync();
        logger.LogInformation("Updated employee {EmpId}", employee.EmpId);
        return existing;
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
