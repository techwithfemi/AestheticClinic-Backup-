using AestheticEMR.Core.Models.Accounting;

namespace AestheticEMR.Core.Services.Accounting.Interfaces;

public interface IAccountingReportLookupService
{
    Task<IEnumerable<vwProfitAndLossHeadersList>> GetProfitAndLossHeadersAsync(CancellationToken ct = default);
    Task<IEnumerable<vwBalanceSheetHeader>> GetBalanceSheetHeadersAsync(CancellationToken ct = default);
}
