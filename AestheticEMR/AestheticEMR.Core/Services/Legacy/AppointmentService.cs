using AestheticEMR.Core.Infrastructure;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Account;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core.Services.Legacy;

public class AppointmentService(ApplicationDbContext context, IUserIdAccessor userIdAccessor) : IAppointmentService
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

    public async Task<hAppointment> CreateAsync(hAppointment appointment)
    {
        appointment.pno = NormalizeText(appointment.pno) ?? throw new InvalidOperationException("Patient number is required.");
        appointment.clinicType = NormalizeText(appointment.clinicType) ?? throw new InvalidOperationException("Clinic type is required.");

        await EnsurePatientExistsAsync(appointment.pno);
        ApplyDefaults(appointment);

        context.hAppointments.Add(appointment);
        await context.SaveChangesAsync();
        return appointment;
    }

    public async Task<hAppointment> UpdateAsync(hAppointment appointment)
    {
        appointment.pno = NormalizeText(appointment.pno) ?? throw new InvalidOperationException("Patient number is required.");
        appointment.clinicType = NormalizeText(appointment.clinicType) ?? throw new InvalidOperationException("Clinic type is required.");

        await EnsurePatientExistsAsync(appointment.pno);
        ApplyDefaults(appointment);

        context.hAppointments.Update(appointment);
        await context.SaveChangesAsync();
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
    private async Task EnsurePatientExistsAsync(string pno)
    {
        var exists = await context.HPatients.AsNoTracking().AnyAsync(x => x.Pno == pno);
        if (!exists)
        {
            throw new InvalidOperationException($"Patient '{pno}' was not found.");
        }
    }

    private void ApplyDefaults(hAppointment appointment)
    {
        appointment.entryDate ??= DateTime.Now;
        appointment.entryTime ??= DateTime.Now;
        appointment.remarks = NormalizeText(appointment.remarks);
        appointment.EmpID = NormalizeText(appointment.EmpID) ?? userIdAccessor.GetCurrentUserEmpId();
    }

    private static string? NormalizeText(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}

