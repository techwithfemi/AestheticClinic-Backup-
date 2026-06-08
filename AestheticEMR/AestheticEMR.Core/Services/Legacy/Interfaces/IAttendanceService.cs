using AestheticEMR.Core.Models.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IAttendanceService
{
    Task<IEnumerable<HRecord>> GetAllAsync();
    Task<IEnumerable<QryhvisitsForToday>> GetTodayVisitsAsync();
    Task<HRecord?> GetByIdAsync(string consultId);
    Task<HRecord> CreateAsync(HRecord record);
    Task<HRecord> UpdateAsync(HRecord record);
    Task DeleteAsync(string consultId);
    Task<IEnumerable<string>> GetClinicTypesAsync();
    Task<string?> GetConsultingNotesAsync(string consultId);
}
