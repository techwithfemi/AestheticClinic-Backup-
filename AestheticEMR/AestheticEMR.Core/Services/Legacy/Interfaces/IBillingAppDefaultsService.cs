using AestheticEMR.Core.Services.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IBillingAppDefaultsService
{
    Task<BillingAppDefaults> GetAsync(CancellationToken cancellationToken = default);
    Task<BillingAppDefaults> ReloadAsync(CancellationToken cancellationToken = default);
}
