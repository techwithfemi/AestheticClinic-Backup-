using System.Data;
using System.Data.Common;
using AestheticEMR.Core.Models.Legacy;
using AestheticEMR.Core.Services.Legacy.Interfaces;
using AestheticEMR.Core.Services.Legacy.Messaging;

namespace AestheticEMR.Core.Services.Legacy;

/// <summary>
/// Cross-database billing sync using MassTransit Outbox (separate-machines topology).
/// The Outbox guarantees at-least-once delivery even if the broker is temporarily unavailable.
/// Consumers in SmartHR and Accounting will apply idempotent upserts/deletes.
/// </summary>
public class MassTransitBillingCrossDatabaseSyncService(
    BillingEventPublisher publisher,
    IBillingCrossDatabaseSyncStrategyProvider strategyProvider) : IBillingCrossDatabaseSyncService
{
    public BillingCrossDatabaseSyncStatus GetStatus(string primaryConnectionString) =>
        strategyProvider.CurrentStatus;

    public async Task SyncCreateOrUpdateAsync(
        DbConnection connection,
        DbTransaction transaction,
        Billing billing,
        IReadOnlyCollection<BillingDetail> details,
        CancellationToken cancellationToken = default)
    {
        // Publish inside the same EF Core SaveChanges scope so MassTransit Outbox
        // persists the outbox message atomically with the billing record.
        await publisher.PublishUpsertedAsync(billing, details, cancellationToken);
    }

    public async Task SyncDeleteAsync(
        DbConnection connection,
        DbTransaction transaction,
        string billNo,
        string pNo,
        CancellationToken cancellationToken = default)
    {
        await publisher.PublishDeletedAsync(billNo, pNo, cancellationToken);
    }
}
