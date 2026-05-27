namespace AestheticEMR.Core.Services.Legacy.Messaging.Events;

public record BillingDeletedEvent
{
    public required string BillNo { get; init; }
    public required string PNo { get; init; }
    public IReadOnlyCollection<string> TranIds { get; init; } = [];
}
