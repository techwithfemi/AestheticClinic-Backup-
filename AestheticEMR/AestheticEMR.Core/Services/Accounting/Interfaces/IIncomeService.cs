using AestheticEMR.Core.Services.Accounting.Models;

namespace AestheticEMR.Core.Services.Accounting.Interfaces;

public interface IIncomeService
{
    Task<PagedIncomeResult> GetPagedAsync(IncomeListQuery query, CancellationToken ct = default);
    Task<IncomeEntry?> GetByIdAsync(long sNo, CancellationToken ct = default);
    Task<List<IncomeEntry>> GetByTranIdAsync(string tranId, CancellationToken ct = default);
    Task<IncomeTranIdResult> GenerateTranIdAsync(CancellationToken ct = default);
    Task<List<IncomeAccountLookup>> GetIncomeAccountsAsync(CancellationToken ct = default);
    Task<List<IncomeAccountLookup>> GetReceivingAccountsAsync(CancellationToken ct = default);
    Task<List<JournalLine>> GetTransactionLinesByTranIdAsync(string tranId, CancellationToken ct = default);
    Task<IncomeEntry> CreateAsync(IncomeEntry entry, string currentUserName, CancellationToken ct = default);
    Task<IncomeBatchSaveResult> CreateBatchAsync(IncomeBatchSaveRequest request, string currentUserName, CancellationToken ct = default);
    Task<IncomeEntry> UpdateAsync(IncomeEntry entry, string currentUserName, CancellationToken ct = default);
    Task<IncomeBatchSaveResult> UpdateByTranIdAsync(string tranId, IncomeBatchSaveRequest request, string currentUserName, CancellationToken ct = default);
    Task DeleteAsync(long sNo, CancellationToken ct = default);
    Task DeleteByTranIdAsync(string tranId, string currentUserName, string period, string coyID, CancellationToken ct = default);
}
