using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Services.Legacy;

public class AttendanceService(ApplicationDbContext context) : IAttendanceService
{
    public async Task<IEnumerable<HRecord>> GetAllAsync()
    {
        return await context.HRecords
            .AsNoTracking()
            .OrderByDescending(x => x.RecDate)
            .ThenByDescending(x => x.Htime)
            .ThenByDescending(x => x.EntryTime)
            .ToListAsync();
    }

    public async Task<HRecord?> GetByIdAsync(string consultId)
    {
        return await context.HRecords.FirstOrDefaultAsync(x => x.ConsultId == consultId);
    }

    public async Task<HRecord> CreateAsync(HRecord record)
    {
        record.ConsultId = await GenerateConsultIdAsync();
        await PopulatePatientDetailsAsync(record);
        ApplyDefaults(record);

        context.HRecords.Add(record);
        await context.SaveChangesAsync();
        return record;
    }

    public async Task<HRecord> UpdateAsync(HRecord record)
    {
        await PopulatePatientDetailsAsync(record);
        ApplyDefaults(record);

        context.HRecords.Update(record);
        await context.SaveChangesAsync();
        return record;
    }

    public async Task DeleteAsync(string consultId)
    {
        var record = await GetByIdAsync(consultId);
        if (record is null)
        {
            return;
        }

        context.HRecords.Remove(record);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetClinicTypesAsync()
    {
        return await context.ClinicTypes
            .AsNoTracking()
            .Where(x => !string.IsNullOrWhiteSpace(x.ClinicName))
            .Select(x => x.ClinicName)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();
    }

    private async Task PopulatePatientDetailsAsync(HRecord record)
    {
        record.PNo = NormalizeText(record.PNo) ?? throw new InvalidOperationException("Patient number is required.");

        var patient = await context.HPatients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Pno == record.PNo);

        if (patient is null)
        {
            throw new InvalidOperationException($"Patient '{record.PNo}' was not found.");
        }

        record.ClientCat = patient.ClientCatId;
        record.Coyname = patient.CoyName;
        record.HmoRef ??= patient.HmoRef;
    }

    private static void ApplyDefaults(HRecord record)
    {
        record.RecDate = record.RecDate == default ? DateTime.Today : record.RecDate;
        record.Htime ??= DateTime.Now;
        record.EntryDate ??= DateTime.Now;
        record.EntryTime ??= DateTime.Now;
        record.BillDate ??= record.RecDate;
        record.Referal = NormalizeText(record.Referal) ?? "NO";
        record.AttndStatus = NormalizeText(record.AttndStatus) ?? "NORMAL";
        record.ClinicType = NormalizeText(record.ClinicType) ?? throw new InvalidOperationException("Clinic type is required.");
        record.Remarks = NormalizeText(record.Remarks);
        record.DocAssigned = NormalizeText(record.DocAssigned);
        record.EmpId = NormalizeText(record.EmpId);
        record.Diagnosis = NormalizeText(record.Diagnosis);
        record.Tariff = NormalizeText(record.Tariff);
        record.Coyname = NormalizeText(record.Coyname);
        record.ClientCat = NormalizeText(record.ClientCat);
        record.HmoRef = NormalizeText(record.HmoRef);
    }

    private static string? NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private async Task<string> GenerateConsultIdAsync()
    {
        return await GetGeneratedIdNoAsync("ConsultID2");
    }

    /// <summary>
    /// C# equivalent of the VB6 <c>getIDNo</c> routine for legacy identity generation.
    /// It executes the <c>getIDNo</c> stored procedure and returns the single generated value.
    /// </summary>
    private async Task<string> GetGeneratedIdNoAsync(string destName)
    {
        var results = await context.Database
            .SqlQuery<string>($"EXEC getIDNo @DestName = {destName}")
            .ToListAsync();

        var generatedId = results.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(generatedId))
        {
            throw new InvalidOperationException(
                $"Stored procedure 'getIDNo' returned no value for destination '{destName}'.");
        }

        return generatedId;
    }
}
