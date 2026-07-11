using AestheticEMR.Core.Services.Legacy.Models;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IShiftDetailService
{
    Task<IEnumerable<ShiftDetailItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ShiftDetailItem?> GetByIdAsync(long shiftId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ShiftLookupItem>> GetShiftLookupsAsync(CancellationToken cancellationToken = default);
    Task<ShiftDetailItem> CreateAsync(ShiftDetailItem item, string currentUserName, CancellationToken cancellationToken = default);
    Task<ShiftDetailItem> UpdateAsync(ShiftDetailItem item, string currentUserName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long shiftId, string currentUserName, CancellationToken cancellationToken = default);
}
