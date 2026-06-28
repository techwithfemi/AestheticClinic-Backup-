using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Employees;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Employees.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Employees;

public class DepartmentService(ApplicationDbContext context, ILogger<DepartmentService> logger) : IDepartmentService
{
    // Mirrors the legacy VB.NET genIDNo() — key in the shared IDgen table.
    private const string DeptIdCode = "Department";

    // Legacy format: zero-padded 2 chars (e.g. "01", "10", "99").
    // VB used Microsoft.VisualBasic.Right("00" & CStr(iDNo), 2), so anything beyond 99
    // would have been silently truncated to the last 2 digits. We reject that instead —
    // a clinic with >99 departments needs a schema change, not silent corruption.
    private const int MaxDepartmentId = 99;
    private const string IdFormat = "00";

    public async Task<string> GenerateDepartmentIdAsync()
    {
        var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == DeptIdCode);
        var nextId = (idgen?.Id ?? 0) + 1;
        if (nextId > MaxDepartmentId)
            throw new InvalidOperationException(
                $"Department id limit reached ({MaxDepartmentId}). Cannot generate a new department.");
        return nextId.ToString(IdFormat);
    }

    public async Task<IEnumerable<EmpDepartments>> GetAllAsync()
    {
        return await context.EmpDepartments
            .AsNoTracking()
            .OrderBy(d => d.DeptId)
            .ToListAsync();
    }

    public async Task<EmpDepartments?> GetByIdAsync(string deptId)
    {
        if (string.IsNullOrWhiteSpace(deptId))
            return null;
        return await context.EmpDepartments.AsNoTracking()
            .FirstOrDefaultAsync(d => d.DeptId == deptId);
    }

    public async Task<EmpDepartments> CreateAsync(EmpDepartments department)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == DeptIdCode);

            decimal nextId;
            if (idgen == null)
            {
                // First-ever department: seed the counter from whatever the table already
                // holds so we don't collide with legacy rows (or with the seed VB.NET data).
                var existingIds = await context.EmpDepartments
                    .Where(d => d.DeptId != null && d.DeptId != string.Empty)
                    .Select(d => d.DeptId)
                    .ToListAsync();

                var maxExisting = existingIds
                    .Select(s => { return int.TryParse(s, out var n) ? n : 0; })
                    .DefaultIfEmpty(0)
                    .Max();

                nextId = maxExisting + 1;
                if (nextId > MaxDepartmentId)
                    throw new InvalidOperationException(
                        $"Department id limit reached ({MaxDepartmentId}). Cannot generate a new department.");

                idgen = new Idgen { DestName = DeptIdCode, Id = nextId };
                context.HrIdgens.Add(idgen);
                logger.LogInformation(
                    "Seeded IDgen for {Code} at id={Id} (derived from existing rows)",
                    DeptIdCode, nextId);
            }
            else
            {
                nextId = idgen.Id + 1;
                if (nextId > MaxDepartmentId)
                    throw new InvalidOperationException(
                        $"Department id limit reached ({MaxDepartmentId}). Cannot generate a new department.");
                idgen.Id = nextId;
                // idgen is already tracked from the query above; no Update() needed.
            }

            // Always overwrite any client-supplied id — server is the source of truth.
            department.DeptId = nextId.ToString(IdFormat);

            // Guard against collisions when a row already exists for the next id
            // (e.g. seed data inserted with manual ids that overlap with the counter).
            var exists = await context.EmpDepartments.AnyAsync(d => d.DeptId == department.DeptId);
            if (exists)
                throw new InvalidOperationException(
                    $"Department id '{department.DeptId}' already exists. Resolve the conflict and retry.");

            context.EmpDepartments.Add(department);
            await context.SaveChangesAsync();
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
        // Bulletproof update: only touch the columns we own (DeptName, DeptAddress, Location),
        // keep the PK intact. Matches the DesignationService pattern.
        var rows = await context.EmpDepartments
            .Where(d => d.DeptId == department.DeptId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(d => d.DeptName, department.DeptName)
                .SetProperty(d => d.DeptAddress, department.DeptAddress)
                .SetProperty(d => d.Location, department.Location));

        if (rows == 0)
            throw new KeyNotFoundException($"Department {department.DeptId} not found.");

        var refreshed = await context.EmpDepartments.AsNoTracking()
            .FirstOrDefaultAsync(d => d.DeptId == department.DeptId);

        logger.LogInformation("Updated department {DeptId}", department.DeptId);
        return refreshed ?? department;
    }

    public async Task<bool> DeleteAsync(string deptId)
    {
        var existing = await context.EmpDepartments.FirstOrDefaultAsync(d => d.DeptId == deptId);
        if (existing == null)
            return false;

        if (await IsInUseAsync(deptId))
            throw new InvalidOperationException(
                $"Department '{deptId}' is currently assigned to one or more employees and cannot be deleted.");

        context.EmpDepartments.Remove(existing);
        await context.SaveChangesAsync();
        logger.LogInformation("Deleted department {DeptId}", deptId);
        return true;
    }

    public async Task<bool> IsInUseAsync(string deptId)
    {
        if (string.IsNullOrWhiteSpace(deptId))
            return false;

        return await context.HrEmployees
            .AsNoTracking()
            .AnyAsync(e => e.DeptId == deptId);
    }

    public async Task<IReadOnlyDictionary<string, int>> GetInUseCountsAsync()
    {
        return await context.HrEmployees
            .AsNoTracking()
            .Where(e => e.DeptId != null && e.DeptId != string.Empty)
            .GroupBy(e => e.DeptId)
            .Select(g => new { DeptId = g.Key!, Count = g.Count() })
            .ToDictionaryAsync(x => x.DeptId, x => x.Count);
    }
}
