using AestheticEMR.Core.Models.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<hAppointment>> GetAllAsync();
    Task<hAppointment?> GetByIdAsync(long id);
    Task<hAppointment> CreateAsync(hAppointment appointment, bool sendSms = true);
    Task<hAppointment> UpdateAsync(hAppointment appointment, bool sendSms = true);
    Task DeleteAsync(long id);
    Task<IEnumerable<string>> GetClinicTypesAsync();
    Task<IEnumerable<vwEmpName>> GetEmployeesAsync();
}
