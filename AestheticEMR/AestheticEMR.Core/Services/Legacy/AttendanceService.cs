using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services;
using AestheticEMR.Core.Services.Account;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Legacy;

public class AttendanceService(
    ApplicationDbContext context,
    IUserIdAccessor userIdAccessor,
    ILogger<AttendanceService> logger,
    IEmrAppDefaultsService emrAppDefaultsService,
    ISmsSender smsSender,
    ISmsTemplateService smsTemplateService) : IAttendanceService
{
    private readonly ILogger<AttendanceService> _logger = logger;
    private readonly IEmrAppDefaultsService _emrAppDefaultsService = emrAppDefaultsService;
    private readonly ISmsSender? _smsSender = smsSender;
    private readonly ISmsTemplateService? _smsTemplateService = smsTemplateService;

    public AttendanceService(ApplicationDbContext context, IUserIdAccessor userIdAccessor)
        : this(context, userIdAccessor, null!, null!, null!, null!)
    {
    }

    public async Task<IEnumerable<HRecord>> GetAllAsync()
    {
        return await context.HRecords
            .AsNoTracking()
            .OrderByDescending(x => x.RecDate)
            .ThenByDescending(x => x.Htime)
            .ThenByDescending(x => x.EntryTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<QryhvisitsForToday>> GetTodayVisitsAsync()
    {
        return await context.QryhvisitsForTodays
            .AsNoTracking()
            .OrderByDescending(x => x.RecDate)
            .ThenBy(x => x.Fullname)
            .ThenBy(x => x.ConsultId)
            .ToListAsync();
    }

    public async Task<HRecord?> GetByIdAsync(string consultId)
    {
        return await context.HRecords.FirstOrDefaultAsync(x => x.ConsultId == consultId);
    }

    public async Task<string?> GetConsultingNotesAsync(string consultId)
    {
        var rows = await context.VwhConsultingDetailsForBillingAlts
            .AsNoTracking()
            .Where(x => x.ConsultId == consultId)
            .OrderByDescending(x => x.CDate)
            .ToListAsync();

        if (rows.Count == 0)
            return null;

        var strPrescX2 = new System.Text.StringBuilder();

        foreach (var row in rows)
        {
            var docName = row.Treatedby ?? "";
            var cDateStr = row.CDate.HasValue ? row.CDate.Value.ToString("dd-MMM-yyyy") : "";
            var cTimeStr = row.CTime ?? "";

            var strPrescX = new System.Text.StringBuilder();
            strPrescX.AppendLine($"Clinic: {row.ClinicType}");
            strPrescX.AppendLine($"Purpose: {row.Purpose ?? ""}");
            strPrescX.AppendLine($"Diagnosis: {row.Diagnosis ?? ""}");
            strPrescX.AppendLine($"##### Treatment by Dr. {docName} on {cDateStr} {cTimeStr} #####");
            strPrescX.AppendLine();

            if (!string.IsNullOrWhiteSpace(row.Investigate))
                strPrescX.Append(row.Investigate);

            if (!string.IsNullOrWhiteSpace(row.Prescription))
            {
                strPrescX.AppendLine();
                strPrescX.Append(row.Prescription);
            }

            if (!string.IsNullOrWhiteSpace(row.Services))
            {
                strPrescX.AppendLine();
                strPrescX.AppendLine(row.Services);
            }

            if (!string.IsNullOrWhiteSpace(row.BillRemarks))
                strPrescX.AppendLine($"-----BILL ADVICE---{row.BillRemarks}");

            strPrescX.AppendLine();
            strPrescX.AppendLine("--------------------------------------");

            strPrescX2.Append(strPrescX);
            strPrescX2.AppendLine("--------------------------------------");
        }

        var result = strPrescX2.ToString().Trim();
        return result.Length > 0 ? result : null;
    }

    public async Task<IEnumerable<VwhConsultingDetailsForBillingAlt>> GetConsultingDetailsAsync(string consultId)
    {
        var normalizedConsultId = NormalizeText(consultId);
        if (string.IsNullOrWhiteSpace(normalizedConsultId))
            return [];

        return await context.VwhConsultingDetailsForBillingAlts
            .AsNoTracking()
            .Where(x => x.ConsultId == normalizedConsultId)
            .OrderByDescending(x => x.CDate)
            .ThenByDescending(x => x.CTime)
            .ToListAsync();
    }

    public async Task<HRecord> CreateAsync(HRecord record, bool sendSms = true)
    {
        var patient = await PopulatePatientDetailsAsync(record);
        ApplyDefaults(record);

        var existingForDay = await GetExistingAttendanceForPatientDayAsync(record.PNo, record.RecDate);
        if (existingForDay is not null)
        {
            await SaveDebtAsync(record.PNo, existingForDay.ConsultId);

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
            await SaveBillAsync(existingForDay);

            if (sendSms)
            {
                await TrySendAttendanceSmsAsync(existingForDay, patient, "recorded");
            }

            return existingForDay;
        }

        record.ConsultId = await GenerateConsultIdAsync();
        await SaveDebtAsync(record.PNo, record.ConsultId);

        context.HRecords.Add(record);
        await UpsertAttendanceBillAccumAsync(record);
        await UpdatePatientLastVisitAsync(record);
        await context.SaveChangesAsync();
        await SaveBillAsync(record);

        if (sendSms)
        {
            await TrySendAttendanceSmsAsync(record, patient, "recorded");
        }

        return record;
    }

    public async Task<HRecord> UpdateAsync(HRecord record, bool sendSms = true)
    {
        var patient = await PopulatePatientDetailsAsync(record);
        ApplyDefaults(record);

        await UpsertAttendanceBillAccumAsync(record);
        await UpdatePatientLastVisitAsync(record);
        await context.SaveChangesAsync();

        if (sendSms)
        {
            await TrySendAttendanceSmsAsync(record, patient, "updated");
        }

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

    private async Task<HPatient> PopulatePatientDetailsAsync(HRecord record)
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

        return patient;
    }

    private async Task TrySendAttendanceSmsAsync(HRecord record, HPatient patient, string action)
    {
        if (_smsSender is null || _smsTemplateService is null)
        {
            _logger?.LogWarning("Skipping attendance SMS because SMS sender or template service is not configured");
            return;
        }

        var patientPhone = NormalizePhoneNumber(patient.PPhoneNo ?? patient.Nokphone);
        if (string.IsNullOrWhiteSpace(patientPhone))
        {
            _logger?.LogInformation("Skipping attendance SMS for patient {PatientNo} because no phone number is available", record.PNo);
            return;
        }

        var patientName = BuildPatientDisplayName(patient);
        var message = _smsTemplateService.BuildAttendanceMessage(
            patientName,
            record.RecDate,
            record.ClinicType,
            record.ConsultId,
            action);

        var (success, messageId, errorMsg) = await _smsSender.SendSmsMessageAsync(patientPhone, message);

        if (!success)
        {
            _logger?.LogWarning("Attendance SMS send failed for patient {PatientNo} ({Phone}): {Error}", record.PNo, patientPhone, errorMsg ?? "Unknown error");
            return;
        }

        _logger?.LogInformation("Attendance SMS sent for patient {PatientNo} ({Phone}), messageId: {MessageId}", record.PNo, patientPhone, messageId ?? "n/a");
    }

    private static string BuildPatientDisplayName(HPatient patient)
    {
        var fullName = string.Join(" ", new[]
        {
            NormalizeText(patient.Title),
            NormalizeText(patient.PFirstname),
            NormalizeText(patient.PSurName)
        }.Where(x => !string.IsNullOrWhiteSpace(x)));

        return string.IsNullOrWhiteSpace(fullName)
            ? "Patient"
            : fullName;
    }

    private static string? NormalizePhoneNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim().Replace(" ", string.Empty).Replace("-", string.Empty);

        if (normalized.StartsWith("00", StringComparison.Ordinal))
        {
            normalized = $"+{normalized[2..]}";
        }

        if (!normalized.StartsWith("+", StringComparison.Ordinal))
        {
            normalized = $"+{normalized.TrimStart('+')}";
        }

        return normalized;
    }

    private static string? NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private void ApplyDefaults(HRecord record)
    {
        // Ensure RecDate stores date-only (time set to 00:00:00) so the DB view
        // qryhvisitsForToday which filters by CURDATE() matches the stored value.
        record.RecDate = record.RecDate == default ? DateTime.Today : record.RecDate.Date;
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

    // --- New: SaveDebtAsync ---
    private async Task SaveDebtAsync(string pNo, string? currentBillNo = null)
    {
        try
        {
            _logger?.LogInformation("SaveDebtAsync called for patient: {PatientNo}, currentBillNo: {CurrentBillNo}", pNo, currentBillNo);

            var normalizedCurrentBillNo = NormalizeText(currentBillNo);
            var previousBillQuery = context.Billings.Where(b => b.pNo == pNo);

            if (!string.IsNullOrWhiteSpace(normalizedCurrentBillNo))
            {
                previousBillQuery = previousBillQuery.Where(b => b.billNO != normalizedCurrentBillNo);
            }

            var previousBill = await previousBillQuery
                .OrderByDescending(b => b.billNO)
                .FirstOrDefaultAsync();

            decimal openBal = 0;
            var pat = await context.HPatients.FirstOrDefaultAsync(x => x.Pno == pNo);
            if (previousBill != null && pat != null)
            {
                // Check if patient is private by comparing CoyName against the PRIVATE config value (0001)
                var emrDefaults = await _emrAppDefaultsService.GetAsync();
                var privateRetainCode = emrDefaults.Get("PRIVATE", "0001");
                var isPrivate = (pat.CoyName ?? string.Empty).Trim() == privateRetainCode;
                
                _logger?.LogInformation("Patient {PatientNo} isPrivate: {IsPrivate}, CoyName: {CoyName}, PrivateRetainCode: {PrivateRetainCode}", 
                    pNo, isPrivate, pat.CoyName, privateRetainCode);
                
                if (isPrivate)
                {
                    var billed = previousBill.AmountBilled ?? 0;
                    var debtBf = previousBill.DebtBF ?? 0;
                    var discount = previousBill.Discount ?? 0;
                    var paid = previousBill.AmountPaid ?? 0;
                    var tax = Convert.ToDecimal(previousBill.Tax ?? 0d);
                    openBal = ((billed - discount) + debtBf + tax) - paid;
                    _logger?.LogInformation("Calculated carry-forward debt for patient {PatientNo} from bill {BillNo}: {OpenBal}", pNo, previousBill.billNO, openBal);
                }
            }

            if (pat != null)
            {
                pat.IsRev = true;
                pat.DebtBf = openBal;
                pat.Debt = openBal;
                await context.SaveChangesAsync();
                _logger?.LogInformation("Updated DebtBf and Debt for patient {PatientNo} to {OpenBal}", pNo, openBal);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error in SaveDebtAsync for patient {PatientNo}", pNo);
            throw;
        }
    }

    // --- Updated: SaveBillAsync ---
    private async Task SaveBillAsync(HRecord record)
    {
        try
        {
            _logger?.LogInformation($"SaveBillAsync called for consultId: {record.ConsultId}, pNo: {record.PNo}");
            
            var existingBilling = await context.Billings.FirstOrDefaultAsync(b => b.billNO == record.ConsultId);
            var pat = await context.HPatients.FirstOrDefaultAsync(x => x.Pno == record.PNo);
            decimal debtBf = pat?.DebtBf ?? 0;
            
            if (existingBilling != null)
            {
                // Update existing billing with latest debt information
                existingBilling.DebtBF = debtBf;
                _logger?.LogInformation($"Updated existing billing for consultId: {record.ConsultId} with DebtBF: {debtBf}");
                await context.SaveChangesAsync();
                return;
            }
            
            var bill = new Billing
            {
                // DO NOT set ID here! Let the database generate it.
                bDate = DateOnly.FromDateTime(record.EntryDate ?? record.RecDate),
                billNO = record.ConsultId,
                pNo = record.PNo,
                clientID = record.Coyname,
                AmountBilled = 0,
                AmountBilledInWord = string.Empty,
                AmountPaid = 0,
                Discount = 0,
                DebtBF = debtBf,
                BillingMonth = (record.EntryDate ?? record.RecDate).ToString("MMMM"),
                BillingYear = (record.EntryDate ?? record.RecDate).Year,
                isProcess = false
            };
            context.Billings.Add(bill);
            await context.SaveChangesAsync();
            _logger?.LogInformation($"Inserted billing for consultId: {record.ConsultId}, pNo: {record.PNo}");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, $"Error in SaveBillAsync for consultId: {record.ConsultId}, pNo: {record.PNo}");
            throw;
        }
    }
}
