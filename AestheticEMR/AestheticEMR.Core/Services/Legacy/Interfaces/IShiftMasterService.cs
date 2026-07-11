using AestheticEMR.Core.Services.Legacy.Models;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IShiftMasterService
{
    Task<IEnumerable<ShiftMasterItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DepartmentLookupItem>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<ShiftMasterDetail?> GetByIdAsync(long shiftId, CancellationToken cancellationToken = default);
    Task<ShiftMasterDetail> CreateAsync(ShiftMasterSaveRequest request, string currentUserName, CancellationToken cancellationToken = default);
    Task<ShiftMasterDetail> UpdateAsync(long shiftId, ShiftMasterSaveRequest request, string currentUserName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long shiftId, string currentUserName, CancellationToken cancellationToken = default);
}
