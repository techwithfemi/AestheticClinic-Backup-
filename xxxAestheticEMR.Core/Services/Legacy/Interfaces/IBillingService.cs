using AestheticEMR.Core.Models.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IBillingService
{
    Task<IEnumerable<Billing>> GetAllAsync();
    Task<Billing?> GetByBillNoAsync(string billNo);
    Task<IEnumerable<BillingDetail>> GetDetailsAsync(string billNo);
    Task<(Billing Billing, IEnumerable<BillingDetail> Details)> CreateAsync(Billing billing, IEnumerable<BillingDetail> details);
    Task<(Billing Billing, IEnumerable<BillingDetail> Details)> UpdateAsync(string billNo, Billing billing, IEnumerable<BillingDetail> details);
    Task DeleteAsync(string billNo);
}
