using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy;
using System.Data.Common;

namespace AestheticEMR.Core.Services.Legacy.Interfaces;

public interface IBillingCrossDatabaseSyncService
{
    BillingCrossDatabaseSyncStatus GetStatus(string primaryConnectionString);

    Task SyncCreateOrUpdateAsync(
        DbConnection connection,
        DbTransaction transaction,
        Billing billing,
        IReadOnlyCollection<BillingDetail> details,
        CancellationToken cancellationToken = default);

    Task SyncDeleteAsync(
        DbConnection connection,
        DbTransaction transaction,
        string billNo,
        string patientNo,
        CancellationToken cancellationToken = default);
}
