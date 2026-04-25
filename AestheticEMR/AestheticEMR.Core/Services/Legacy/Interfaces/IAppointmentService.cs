using AestheticEMR.Core.Models.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<hAppointment>> GetAllAsync();
    Task<hAppointment?> GetByIdAsync(long id);
    Task<hAppointment> CreateAsync(hAppointment appointment);
    Task<hAppointment> UpdateAsync(hAppointment appointment);
    Task DeleteAsync(long id);
    Task<IEnumerable<string>> GetClinicTypesAsync();
    Task<IEnumerable<vwEmpName>> GetEmployeesAsync();
}
