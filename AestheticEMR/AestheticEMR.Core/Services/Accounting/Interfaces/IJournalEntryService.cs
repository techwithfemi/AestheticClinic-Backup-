using AestheticEMR.Core.Services.Accounting.Models;

namespace AestheticEMR.Core.Services.Accounting.Interfaces;

public interface IJournalEntryService
{
    Task<PagedJournalResult> GetPagedAsync(JournalListQuery query, CancellationToken ct = default);

    Task<JournalEntry?> GetByTranNoAsync(string tranNo, CancellationToken ct = default);

    Task<string> GenerateNextTranNoAsync(CancellationToken ct = default);

    Task<List<JournalAccountLookup>> GetAccountsAsync(CancellationToken ct = default);

    Task<List<JournalCostCenterLookup>> GetCostCentersAsync(CancellationToken ct = default);

    Task<JournalEntry> CreateAsync(JournalEntry entry, string currentUser, CancellationToken ct = default);

    Task<JournalEntry> UpdateAsync(JournalEntry entry, string currentUser, CancellationToken ct = default);

    Task DeleteAsync(string tranNo, string currentUser, CancellationToken ct = default);
}