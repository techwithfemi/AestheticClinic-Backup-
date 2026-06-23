using AestheticEMR.Core.Models.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IAttendanceService
{
    Task<IEnumerable<HRecord>> GetAllAsync();
    Task<IEnumerable<QryhvisitsForToday>> GetTodayVisitsAsync();
    Task<HRecord?> GetByIdAsync(string consultId);
    Task<HRecord> CreateAsync(HRecord record, bool sendSms = true);
    Task<HRecord> UpdateAsync(HRecord record, bool sendSms = true);
    Task DeleteAsync(string consultId);
    Task<IEnumerable<string>> GetClinicTypesAsync();
    Task<string?> GetConsultingNotesAsync(string consultId);
    Task<IEnumerable<VwhConsultingDetailsForBillingAlt>> GetConsultingDetailsAsync(string consultId);
}
