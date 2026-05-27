namespace AestheticEMR.Core.Services.Legacy.Messaging.Events;

public record BillingUpsertedEvent
{
    public required string BillNo { get; init; }
    public required DateOnly BDate { get; init; }
    public required string PNo { get; init; }
    public string? ClientId { get; init; }
    public decimal DebtBF { get; init; }
    public decimal AmountBilled { get; init; }
    public decimal Discount { get; init; }
    public decimal AmountPaid { get; init; }
    public string? BillType { get; init; }
    public bool IsPaid { get; init; }
    public bool IsProcess { get; init; }
    public DateTime? AdmDate { get; init; }
    public DateTime? DischDate { get; init; }
    public DateTime? TimeVal { get; init; }
    public string? ApprvCode { get; init; }
    public bool IsPost { get; init; }
    public IReadOnlyCollection<BillingDetailPayload> Details { get; init; } = [];
}

public record BillingDetailPayload
{
    public required string BillNo { get; init; }
    public long SNO { get; init; }
    public string? TranID { get; init; }
    public DateTime DtDate { get; init; }
    public required string DrgName { get; init; }
    public double Price { get; init; }
    public double Qty { get; init; }
    public decimal? SubTotal { get; init; }
    public string? BillType { get; init; }
    public string? ConId { get; init; }
    public string? RevType { get; init; }
    public string? BillTo { get; init; }
    public string? CoyName { get; init; }
    public string? BillBy { get; init; }
}
