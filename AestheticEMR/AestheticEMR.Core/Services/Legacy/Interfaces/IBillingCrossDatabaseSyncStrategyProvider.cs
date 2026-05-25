using AestheticEMR.Core.Services.Legacy;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IBillingCrossDatabaseSyncStrategyProvider
{
    BillingCrossDatabaseSyncStatus CurrentStatus { get; }
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
