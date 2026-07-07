using AestheticEMR.Core.Services.Accounting.Models;

namespace AestheticEMR.Core.Services.Accounting.Interfaces;

public interface IExpenseService
{
    Task<PagedExpenseResult> GetPagedAsync(ExpenseListQuery query, CancellationToken ct = default);
    Task<ExpenseEntry?> GetByIdAsync(long sNo, CancellationToken ct = default);
    Task<List<ExpenseEntry>> GetByTranIdAsync(string tranId, CancellationToken ct = default);
    Task<ExpenseTranIdResult> GenerateTranIdAsync(CancellationToken ct = default);
    Task<List<ExpenseAccountLookup>> GetExpenseAccountsAsync(CancellationToken ct = default);
    Task<List<ExpenseAccountLookup>> GetPayingAccountsAsync(CancellationToken ct = default);
    Task<List<JournalLine>> GetTransactionLinesByTranIdAsync(string tranId, CancellationToken ct = default);
    Task<ExpenseEntry> CreateAsync(ExpenseEntry entry, string currentUserName, CancellationToken ct = default);
    Task<ExpenseBatchSaveResult> CreateBatchAsync(ExpenseBatchSaveRequest request, string currentUserName, CancellationToken ct = default);
    Task<ExpenseEntry> UpdateAsync(ExpenseEntry entry, string currentUserName, CancellationToken ct = default);
    Task<ExpenseBatchSaveResult> UpdateByTranIdAsync(string tranId, ExpenseBatchSaveRequest request, string currentUserName, CancellationToken ct = default);
    Task DeleteAsync(long sNo, CancellationToken ct = default);
    Task DeleteByTranIdAsync(string tranId, string currentUserName, string period, string coyID, CancellationToken ct = default);
}
