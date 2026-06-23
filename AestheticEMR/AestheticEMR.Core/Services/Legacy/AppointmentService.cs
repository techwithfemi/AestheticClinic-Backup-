using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services;
using AestheticEMR.Core.Services.Account;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AestheticEMR.Core.Services.Legacy;

public class AppointmentService(
    ApplicationDbContext context,
    IUserIdAccessor userIdAccessor,
    ISmsSender smsSender,
    ISmsTemplateService smsTemplateService,
    ILogger<AppointmentService> logger) : IAppointmentService
{
    public async Task<IEnumerable<hAppointment>> GetAllAsync()
    {
        return await context.hAppointments
            .AsNoTracking()
            .OrderByDescending(x => x.ApptDate)
            .ThenByDescending(x => x.ApptTime)
            .ToListAsync();
    }

    public async Task<hAppointment?> GetByIdAsync(long id)
    {
        return await context.hAppointments.FirstOrDefaultAsync(x => x.ID == id);
    }

    public async Task<hAppointment> CreateAsync(hAppointment appointment, bool sendSms = true)
    {
        appointment.pno = NormalizeText(appointment.pno) ?? throw new InvalidOperationException("Patient number is required.");
        appointment.clinicType = NormalizeText(appointment.clinicType) ?? throw new InvalidOperationException("Clinic type is required.");

        var patient = await GetPatientAsync(appointment.pno);
        ApplyDefaults(appointment);

        context.hAppointments.Add(appointment);
        await context.SaveChangesAsync();

        if (sendSms)
        {
            await TrySendAppointmentSmsAsync(appointment, patient, "scheduled");
        }

        return appointment;
    }

    public async Task<hAppointment> UpdateAsync(hAppointment appointment, bool sendSms = true)
    {
        appointment.pno = NormalizeText(appointment.pno) ?? throw new InvalidOperationException("Patient number is required.");
        appointment.clinicType = NormalizeText(appointment.clinicType) ?? throw new InvalidOperationException("Clinic type is required.");

        var patient = await GetPatientAsync(appointment.pno);
        ApplyDefaults(appointment);

        context.hAppointments.Update(appointment);
        await context.SaveChangesAsync();

        if (sendSms)
        {
            await TrySendAppointmentSmsAsync(appointment, patient, "updated");
        }

        return appointment;
    }

    public async Task DeleteAsync(long id)
    {
        var appointment = await GetByIdAsync(id);
        if (appointment is null)
        {
            return;
        }

        context.hAppointments.Remove(appointment);
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

    public async Task<IEnumerable<vwEmpName>> GetEmployeesAsync()
    {
        return await context.vwEmpNames
            .AsNoTracking()
            .OrderBy(x => x.EmpName)
            .ToListAsync();
    }

    private async Task<HPatient> GetPatientAsync(string pno)
    {
        var patient = await context.HPatients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Pno == pno);

        return patient ?? throw new InvalidOperationException($"Patient '{pno}' was not found.");
    }

    private void ApplyDefaults(hAppointment appointment)
    {
        appointment.entryDate ??= DateTime.Now;
        appointment.entryTime ??= DateTime.Now;
        appointment.remarks = NormalizeText(appointment.remarks);
        appointment.EmpID = NormalizeText(appointment.EmpID) ?? userIdAccessor.GetCurrentUserEmpId();
    }

    private async Task TrySendAppointmentSmsAsync(hAppointment appointment, HPatient patient, string action)
    {
        var patientPhone = NormalizePhoneNumber(patient.PPhoneNo ?? patient.Nokphone);
        if (string.IsNullOrWhiteSpace(patientPhone))
        {
            logger.LogInformation("Skipping appointment SMS for patient {PatientNo} because no phone number is available", appointment.pno);
            return;
        }

        var patientName = BuildPatientDisplayName(patient);
        var message = smsTemplateService.BuildAppointmentMessage(
            patientName,
            appointment.ApptDate,
            appointment.ApptTime,
            appointment.clinicType,
            action);

        var (success, messageId, errorMsg) = await smsSender.SendSmsMessageAsync(patientPhone, message);

        if (!success)
        {
            logger.LogWarning("Appointment SMS send failed for patient {PatientNo} ({Phone}): {Error}", appointment.pno, patientPhone, errorMsg ?? "Unknown error");
            return;
        }

        logger.LogInformation("Appointment SMS sent for patient {PatientNo} ({Phone}), messageId: {MessageId}", appointment.pno, patientPhone, messageId ?? "n/a");
    }

    private static string BuildAppointmentSmsMessage(string patientName, hAppointment appointment, string action)
    {
        var dateText = appointment.ApptDate?.ToString("dd MMM yyyy") ?? "N/A";
        var timeText = appointment.ApptTime?.ToString("hh:mm tt") ?? "N/A";
        var clinicText = NormalizeText(appointment.clinicType) ?? "clinic";

        return $"Hello {patientName}, your appointment has been {action}. Date: {dateText}, Time: {timeText}, Clinic: {clinicText}. Please contact us if you need any changes.";
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
}

