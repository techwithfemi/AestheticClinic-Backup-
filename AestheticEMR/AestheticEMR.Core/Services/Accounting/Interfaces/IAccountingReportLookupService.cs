using AestheticEMR.Core.Models.Accounting;
using AestheticEMR.Core.Services.Accounting.Models;

namespace AestheticEMR.Core.Services.Accounting.Interfaces;

public interface IAccountingReportLookupService
{
    Task<IEnumerable<vwProfitAndLossHeadersList>> GetProfitAndLossHeadersAsync(CancellationToken ct = default);
    Task<IEnumerable<vwBalanceSheetHeader>> GetBalanceSheetHeadersAsync(CancellationToken ct = default);
    Task<IEnumerable<AccountingReportYearLookup>> GetGeneralLedgerYearsAsync(CancellationToken ct = default);
    Task<IEnumerable<AccountingReportPeriodLookup>> GetGeneralLedgerPeriodsAsync(string coyID, string year, CancellationToken ct = default);
    Task<IEnumerable<AccountingLedgerLookup>> GetGeneralLedgerLedgersAsync(CancellationToken ct = default);
    Task<IEnumerable<AccountingAccountLookup>> GetGeneralLedgerAccountsAsync(string coyID, string period, string ledgerCode, CancellationToken ct = default);
    Task<string?> GetCompanyNameAsync(string coyID, CancellationToken ct = default);
}
