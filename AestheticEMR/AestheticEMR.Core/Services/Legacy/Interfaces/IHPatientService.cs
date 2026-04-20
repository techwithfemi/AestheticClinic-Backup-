using AestheticEMR.Core.Models.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IHPatientService
{
    Task<IEnumerable<HPatient>> GetAllAsync();
    Task<HPatient?> GetByIdAsync(string pno);
    Task<HPatient> CreateAsync(HPatient patient);
    Task<HPatient> UpdateAsync(HPatient patient);
    Task DeleteAsync(string pno);
}
