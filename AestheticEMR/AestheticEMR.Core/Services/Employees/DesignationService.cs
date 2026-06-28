using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Employees;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Employees.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Employees;

public class DesignationService(ApplicationDbContext context, ILogger<DesignationService> logger) : IDesignationService
{
    // Mirrors the legacy VB.NET genIDNo() — key in the shared IDgen table.
    private const string DesIdCode = "Designation";

    // Legacy format: zero-padded 2 chars (e.g. "01", "10", "99").
    // VB used Microsoft.VisualBasic.Right("00" & CStr(iDNo), 2), so anything beyond 99
    // would have been silently truncated to the last 2 digits. We reject that instead —
    // a clinic with >99 designations needs a schema change, not silent corruption.
    private const int MaxDesignationId = 99;
    private const string IdFormat = "00";

    public async Task<string> GenerateDesignationIdAsync()
    {
        var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == DesIdCode);
        var nextId = (idgen?.Id ?? 0) + 1;
        if (nextId > MaxDesignationId)
            throw new InvalidOperationException(
                $"Designation id limit reached ({MaxDesignationId}). Cannot generate a new designation.");
        return nextId.ToString(IdFormat);
    }

    public async Task<IEnumerable<Designation>> GetAllAsync()
    {
        return await context.Designations
            .AsNoTracking()
            .OrderBy(d => d.desID)
            .ToListAsync();
    }

    public async Task<Designation?> GetByIdAsync(string desId)
    {
        if (string.IsNullOrWhiteSpace(desId))
            return null;
        return await context.Designations.AsNoTracking()
            .FirstOrDefaultAsync(d => d.desID == desId);
    }

    public async Task<Designation> CreateAsync(Designation designation)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == DesIdCode);

            decimal nextId;
            if (idgen == null)
            {
                // First-ever designation: seed the counter from whatever the table already
                // holds so we don't collide with legacy rows (or with the seed VB.NET data).
                var existingIds = await context.Designations
                    .Where(d => d.desID != null && d.desID != string.Empty)
                    .Select(d => d.desID)
                    .ToListAsync();

                var maxExisting = existingIds
                    .Select(s => { return int.TryParse(s, out var n) ? n : 0; })
                    .DefaultIfEmpty(0)
                    .Max();

                nextId = maxExisting + 1;
                if (nextId > MaxDesignationId)
                    throw new InvalidOperationException(
                        $"Designation id limit reached ({MaxDesignationId}). Cannot generate a new designation.");

                idgen = new Idgen { DestName = DesIdCode, Id = nextId };
                context.HrIdgens.Add(idgen);
                logger.LogInformation(
                    "Seeded IDgen for {Code} at id={Id} (derived from existing rows)",
                    DesIdCode, nextId);
            }
            else
            {
                nextId = idgen.Id + 1;
                if (nextId > MaxDesignationId)
                    throw new InvalidOperationException(
                        $"Designation id limit reached ({MaxDesignationId}). Cannot generate a new designation.");
                idgen.Id = nextId;
                // idgen is already tracked from the query above; no Update() needed.
            }

            // Always overwrite any client-supplied id — server is the source of truth.
            designation.desID = nextId.ToString(IdFormat);

            // Guard against collisions when a row already exists for the next id
            // (e.g. seed data inserted with manual ids that overlap with the counter).
            var exists = await context.Designations.AnyAsync(d => d.desID == designation.desID);
            if (exists)
                throw new InvalidOperationException(
                    $"Designation id '{designation.desID}' already exists. Resolve the conflict and retry.");

            context.Designations.Add(designation);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("Created designation {DesId}", designation.desID);
            return designation;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Designation> UpdateAsync(Designation designation)
    {
        // Bulletproof update: only touch the column we own (desName), keep the PK intact.
        var rows = await context.Designations
            .Where(d => d.desID == designation.desID)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(d => d.desName, designation.desName));

        if (rows == 0)
            throw new KeyNotFoundException($"Designation {designation.desID} not found.");

        var refreshed = await context.Designations.AsNoTracking()
            .FirstOrDefaultAsync(d => d.desID == designation.desID);

        logger.LogInformation("Updated designation {DesId}", designation.desID);
        return refreshed ?? designation;
    }

    public async Task<bool> DeleteAsync(string desId)
    {
        var existing = await context.Designations.FirstOrDefaultAsync(d => d.desID == desId);
        if (existing == null)
            return false;

        if (await IsInUseAsync(desId))
            throw new InvalidOperationException(
                $"Designation '{desId}' is currently assigned to one or more employees and cannot be deleted.");

        context.Designations.Remove(existing);
        await context.SaveChangesAsync();
        logger.LogInformation("Deleted designation {DesId}", desId);
        return true;
    }

    public async Task<bool> IsInUseAsync(string desId)
    {
        if (string.IsNullOrWhiteSpace(desId))
            return false;

        return await context.HrEmployees
            .AsNoTracking()
            .AnyAsync(e => e.Designation == desId);
    }

    public async Task<IReadOnlyDictionary<string, int>> GetInUseCountsAsync()
    {
        return await context.HrEmployees
            .AsNoTracking()
            .Where(e => e.Designation != null && e.Designation != string.Empty)
            .GroupBy(e => e.Designation)
            .Select(g => new { DesId = g.Key!, Count = g.Count() })
            .ToDictionaryAsync(x => x.DesId, x => x.Count);
    }
}