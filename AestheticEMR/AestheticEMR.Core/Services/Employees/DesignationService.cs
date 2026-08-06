using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Employees;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Employees.Interfaces;
using DataAccess.DbAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Employees;

public class DesignationService(
    ApplicationDbContext context,
    ISqlDataAccess db,
    ILogger<DesignationService> logger) : IDesignationService
{
    // Mirrors the legacy VB.NET genIDNo() — key in the shared IDgen table.
    private const string DesIdCode = "Designation";
    private const string ConnectionId = "smartHRConnection";

    // Legacy format: zero-padded 2 chars (e.g. "01", "10", "99").
    // VB used Microsoft.VisualBasic.Right("00" & CStr(iDNo), 2), so anything beyond 99
    // would have been silently truncated to the last 2 digits. We reject that instead —
    // a clinic with >99 designations needs a schema change, not silent corruption.
    private const int MaxDesignationId = 99;
    private const string IdFormat = "00";

    private async Task<int> GetMaxExistingDesignationIdAsync()
    {
        var query = "SELECT desID FROM Designation WHERE desID IS NOT NULL AND desID <> ''";
        var existingIds = await db.LoadDataText<string, dynamic>(query, new { }, ConnectionId);

        return existingIds
            .Select(s => int.TryParse(s, out var n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();
    }

    public async Task<string> GenerateDesignationIdAsync()
    {
        var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == DesIdCode);
        var maxExisting = await GetMaxExistingDesignationIdAsync();

        var lastUsed = Math.Max((int)(idgen?.Id ?? 0), maxExisting);
        var nextId = lastUsed + 1;

        if (nextId > MaxDesignationId)
            throw new InvalidOperationException(
                $"Designation id limit reached ({MaxDesignationId}). Cannot generate a new designation.");

        return nextId.ToString(IdFormat);
    }

    public async Task<IEnumerable<Designation>> GetAllAsync()
    {
        // Calls stored procedure: getDesig
        // Legacy VB.NET: dr = SqlHelper.ExecuteDataset(conStr2, CommandType.StoredProcedure, "getDesig")
        var designations = await db.LoadData<Designation, dynamic>(
            "getDesig",
            new { },
            ConnectionId);

        return designations.OrderBy(d => d.desID);
    }

    public async Task<Designation?> GetByIdAsync(string desId)
    {
        if (string.IsNullOrWhiteSpace(desId))
            return null;

        // Use raw SQL query to get by ID
        var query = "SELECT desID, desName FROM Designation WHERE desID = @desID";
        var results = await db.LoadDataText<Designation, dynamic>(
            query,
            new { desID = desId },
            ConnectionId);

        return results.FirstOrDefault();
    }

    public async Task<Designation> CreateAsync(Designation designation)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var idgen = await context.HrIdgens.FirstOrDefaultAsync(x => x.DestName == DesIdCode);
            var maxExisting = await GetMaxExistingDesignationIdAsync();

            var lastUsed = Math.Max((int)(idgen?.Id ?? 0), maxExisting);
            var nextIdInt = lastUsed + 1;

            if (nextIdInt > MaxDesignationId)
                throw new InvalidOperationException(
                    $"Designation id limit reached ({MaxDesignationId}). Cannot generate a new designation.");

            var nextId = (decimal)nextIdInt;

            if (idgen == null)
            {
                idgen = new Idgen { DestName = DesIdCode, Id = nextId };
                context.HrIdgens.Add(idgen);
                logger.LogInformation(
                    "Seeded IDgen for {Code} at id={Id} (reconciled with existing rows)",
                    DesIdCode, nextId);
            }
            else
            {
                idgen.Id = nextId;
            }

            // Always overwrite any client-supplied id — server is the source of truth.
            designation.desID = nextId.ToString(IdFormat);

            // Guard against collisions when a row already exists for the next id
            var checkQuery = "SELECT COUNT(*) FROM Designation WHERE desID = @desID";
            var existsResults = await db.LoadDataText<int, dynamic>(
                checkQuery,
                new { desID = designation.desID },
                ConnectionId);
            var exists = existsResults.FirstOrDefault() > 0;

            if (exists)
                throw new InvalidOperationException(
                    $"Designation id '{designation.desID}' already exists. Resolve the conflict and retry.");

            // Calls stored procedure: InsertEmpDesig
            // Legacy VB.NET: params = {New SqlParameter("@desID", desID), New SqlParameter("@desName", desName)}
            // SqlHelper.ExecuteNonQuery(conStr2, CommandType.StoredProcedure, "InsertEmpDesig", params)
            await db.SaveData(
                "InsertEmpDesig",
                new { desID = designation.desID, desName = designation.desName },
                ConnectionId);

            await context.SaveChangesAsync(); // Save IDgen changes
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
        // Calls stored procedure: updateEmpDesig
        // Legacy VB.NET: params = {New SqlParameter("@desOldID", str), 
        //                          New SqlParameter("@desID", desID), 
        //                          New SqlParameter("@desName", info)}
        // SqlHelper.ExecuteNonQuery(conStr2, CommandType.StoredProcedure, "updateEmpDesig", params)
        
        // For update, we keep the same ID (desOldID = desID)
        await db.SaveData(
            "updateEmpDesig",
            new { desOldID = designation.desID, desID = designation.desID, desName = designation.desName },
            ConnectionId);

        logger.LogInformation("Updated designation {DesId}", designation.desID);
        
        // Return the updated designation
        return designation;
    }

    public async Task<bool> DeleteAsync(string desId)
    {
        var existing = await GetByIdAsync(desId);
        if (existing == null)
            return false;

        if (await IsInUseAsync(desId))
            throw new InvalidOperationException(
                $"Designation '{desId}' is currently assigned to one or more employees and cannot be deleted.");

        // Calls stored procedure: deleteEmpDesig
        // Legacy VB.NET: params = {New SqlParameter("@desID", info)}
        // SqlHelper.ExecuteNonQuery(conStr2, CommandType.StoredProcedure, "deleteEmpDesig", params)
        await db.SaveData(
            "deleteEmpDesig",
            new { desID = desId },
            ConnectionId);

        logger.LogInformation("Deleted designation {DesId}", desId);
        return true;
    }

    public async Task<bool> IsInUseAsync(string desId)
    {
        if (string.IsNullOrWhiteSpace(desId))
            return false;

        // Check if any employee references this designation
        var query = "SELECT COUNT(*) FROM HREmployees WHERE Designation = @Designation";
        var results = await db.LoadDataText<int, dynamic>(
            query,
            new { Designation = desId },
            ConnectionId);

        return results.FirstOrDefault() > 0;
    }

    public async Task<IReadOnlyDictionary<string, int>> GetInUseCountsAsync()
    {
        // Get count of employees per designation
        var query = @"
            SELECT DesId as DesId, COUNT(*) as Count 
            FROM Designation 
            WHERE DesId IS NOT NULL AND DesId <> '' 
            GROUP BY DesId";

        var results = await db.LoadDataText<DesignationUsageCount, dynamic>(
            query,
            new { },
            ConnectionId);

        return results.ToDictionary(x => x.DesId, x => x.Count);
    }

    private class DesignationUsageCount
    {
        public string DesId { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}