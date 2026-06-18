namespace AestheticEMR.Server.ViewModels.Legacy;

public class QryhBillingIncomeVM
{
    public DateTime ReceiptDate { get; set; }
    public DateTime? RTime { get; set; }
    public string ReceiptNo { get; set; } = string.Empty;
    public string PNo { get; set; } = string.Empty;
    public string PaymentFor { get; set; } = string.Empty;
    public decimal AmountBilled { get; set; }
    public decimal Tax { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal? Balance { get; set; }
    public string PayType { get; set; } = string.Empty;
    public string? ClinicId { get; set; }
    public string Fullname { get; set; } = string.Empty;
    public string PatNo { get; set; } = string.Empty;
    public string? ReceivedBy { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string? CoyName { get; set; }
    public bool? IsPost { get; set; }
    public string? Remarks { get; set; }
    public bool? Suppres { get; set; }

    /// <summary>
    /// False when the billNo (consultId) is still referenced in operational
    /// tables (HRecords, Billings, HDental, HConsulting) or has linked payments
    /// beyond this receipt. Set server-side on list load.
    /// </summary>
    public bool CanDelete { get; set; } = true;
}
