using AestheticEMR.Core.Services.Legacy.Models;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IRosterGroupService
{
    Task<string> GetCurrentDepartmentNameAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RosterGroupDepartmentItem>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RosterGroupGridItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RosterGroupAvailableStaffItem>> GetAvailableStaffAsync(string? deptId = null, CancellationToken cancellationToken = default);
    Task<RosterGroupItem?> GetByIdAsync(long rosterGrpId, CancellationToken cancellationToken = default);
    Task<RosterGroupItem> CreateAsync(RosterGroupSaveRequest request, string currentUserName, CancellationToken cancellationToken = default);
    Task<RosterGroupItem> UpdateAsync(long rosterGrpId, RosterGroupSaveRequest request, string currentUserName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(long rosterGrpId, string currentUserName, CancellationToken cancellationToken = default);
}
