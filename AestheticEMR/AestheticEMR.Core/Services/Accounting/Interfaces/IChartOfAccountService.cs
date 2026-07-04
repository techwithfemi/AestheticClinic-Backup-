using AestheticEMR.Core.Services.Accounting.Models;

namespace AestheticEMR.Core.Services.Accounting.Interfaces;

public interface IChartOfAccountService
{
    Task<PagedChartOfAccountResult> GetPagedAsync(ChartOfAccountListQuery query, CancellationToken ct = default);
    Task<ChartOfAccountEntry?> GetByIdAsync(long sNo, CancellationToken ct = default);
    Task<ChartOfAccountDefaults> GetDefaultsAsync(CancellationToken ct = default);
    Task<List<ChartOfAccountGroupLookup>> GetGroupsAsync(CancellationToken ct = default);
    Task<string> GetNextAccountNoAsync(string groupId, CancellationToken ct = default);
    Task<ChartOfAccountEntry> CreateAsync(ChartOfAccountEntry entry, CancellationToken ct = default);
    Task<ChartOfAccountEntry> UpdateAsync(ChartOfAccountEntry entry, CancellationToken ct = default);
    Task DeleteAsync(long sNo, CancellationToken ct = default);
}
