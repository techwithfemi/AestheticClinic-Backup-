using AestheticEMR.Core.Services.Legacy.Models;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IRosterService
{
    Task<RosterLookups> GetLookupsAsync(string deptId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RosterGridItem>> GetGridAsync(RosterGridQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<RosterGridItem>> GetExistingAsync(RosterEditorQuery query, CancellationToken cancellationToken = default);
    Task<RosterSaveResult> SaveAsync(RosterSaveRequest request, string currentUserName, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(RosterDeleteRequest request, string currentUserName, CancellationToken cancellationToken = default);
}
