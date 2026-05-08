using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Account;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Services.Legacy;

public class AttendanceService(ApplicationDbContext context, IUserIdAccessor userIdAccessor) : IAttendanceService
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
        await PopulatePatientDetailsAsync(record);
        ApplyDefaults(record);

        var existingForDay = await GetExistingAttendanceForPatientDayAsync(record.PNo, record.RecDate);
        if (existingForDay is not null)
        {
            existingForDay.ClinicType = record.ClinicType;
            existingForDay.AttndStatus = record.AttndStatus;
            existingForDay.ClientCat = record.ClientCat;
            existingForDay.Coyname = record.Coyname;
            existingForDay.HmoRef = record.HmoRef;
            existingForDay.RecDate = record.RecDate;
            existingForDay.EntryDate = record.EntryDate;
            existingForDay.EntryTime = record.EntryTime;

            await UpsertAttendanceBillAccumAsync(existingForDay);
            await UpdatePatientLastVisitAsync(existingForDay);
            await context.SaveChangesAsync();
            return existingForDay;
        }

        record.ConsultId = await GenerateConsultIdAsync();
        context.HRecords.Add(record);
        await UpsertAttendanceBillAccumAsync(record);
        await UpdatePatientLastVisitAsync(record);
        await context.SaveChangesAsync();
        return record;
    }

    public async Task<HRecord> UpdateAsync(HRecord record)
    {
        await PopulatePatientDetailsAsync(record);
        ApplyDefaults(record);

        await UpsertAttendanceBillAccumAsync(record);
        await UpdatePatientLastVisitAsync(record);
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

        var hasConsulting = await context.HConsultings.AnyAsync(x => x.ConsultId == consultId);
        var hasDental = await context.HDentals.AnyAsync(x => x.ConsultId == consultId);
        var hasDentalTreat = await context.HDentalTreats.AnyAsync(x => x.ConsultId == consultId);
        var hasBilling = await context.Billings.AnyAsync(x => x.billNO == consultId);
        var hasBillAccum = await context.BillAccums.AnyAsync(x => x.consultID == consultId);

        if (hasConsulting || hasDental || hasDentalTreat || hasBilling || hasBillAccum)
        {
            throw new InvalidOperationException("This attendance is referenced by operational/billing records and cannot be deleted.");
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

    private async Task EnsureSingleAttendancePerPatientPerDayAsync(string pNo, DateTime recDate, string? currentConsultId)
    {
        var targetDate = recDate.Date;

        var duplicateExists = await context.HRecords
            .AnyAsync(x => x.PNo == pNo
                           && x.RecDate.Date == targetDate
                           && (currentConsultId == null || x.ConsultId != currentConsultId));

        if (duplicateExists)
        {
            throw new InvalidOperationException("Only one attendance can be taken per patient per day.");
        }
    }

    private async Task<HRecord?> GetExistingAttendanceForPatientDayAsync(string pNo, DateTime recDate)
    {
        var targetDate = recDate.Date;

        return await context.HRecords.FirstOrDefaultAsync(x =>
            x.PNo == pNo && x.RecDate.Date == targetDate);
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

        var incomingClientCat = NormalizeText(record.ClientCat);
        var incomingCoyName = NormalizeText(record.Coyname);
        var incomingHmoRef = NormalizeText(record.HmoRef);

        record.ClientCat = incomingClientCat ?? patient.ClientCatId;
        record.Coyname = incomingCoyName ?? patient.CoyName;
        record.HmoRef = incomingHmoRef ?? patient.HmoRef;
    }

    private void ApplyDefaults(HRecord record)
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
        record.EmpId = NormalizeText(record.EmpId) ?? userIdAccessor.GetCurrentUserEmpId();
        record.Diagnosis = NormalizeText(record.Diagnosis);
        record.Tariff = NormalizeText(record.Tariff);
        record.Coyname = NormalizeText(record.Coyname);
        record.ClientCat = NormalizeText(record.ClientCat);
        record.HmoRef = NormalizeText(record.HmoRef);
    }

    private async Task UpsertAttendanceBillAccumAsync(HRecord record)
    {
        var consultId = NormalizeText(record.ConsultId) ?? throw new InvalidOperationException("Consult ID is required for billing accumulation.");
        var patientNo = NormalizeText(record.PNo) ?? throw new InvalidOperationException("Patient number is required for billing accumulation.");
        var companyCode = NormalizeText(record.Coyname) ?? string.Empty;

        var billAccum = await context.BillAccums
            .FirstOrDefaultAsync(x => x.consultID == consultId && x.drgName == "ATTENDANCE");

        if (billAccum is null)
        {
            billAccum = new BillAccum
            {
                consultID = consultId,
                drgName = "ATTENDANCE"
            };

            context.BillAccums.Add(billAccum);
        }

        billAccum.dtDate = record.EntryDate ?? record.RecDate;
        billAccum.Price = 0m;
        billAccum.Qty = 1m;
        billAccum.subTotal = 0m;
        billAccum.pNO = patientNo;
        billAccum.billtype = "SERVICE";
        billAccum.conID = null;
        billAccum.CoyName = companyCode;
        billAccum.BillTo = companyCode;
        billAccum.attendedTo = false;
        billAccum.isBilled = false;
        billAccum.revType = "CONSULTATION";
        billAccum.AppVersion = 1;
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

    private async Task UpdatePatientLastVisitAsync(HRecord record)
    {
        var patient = await context.HPatients
            .FirstOrDefaultAsync(x => x.Pno == record.PNo);

        if (patient is null)
            return;

        patient.LastPurpose = record.Remarks;
        patient.LastClinicVisited = record.ClinicType;
        patient.LastAttndDate = record.EntryDate ?? record.RecDate;
        patient.LastConsultId = record.ConsultId;
    }
}
