namespace AestheticEMR.Server.ViewModels.Legacy;

public class ReceiptListVM
{
    public string ReceiptNo { get; set; } = string.Empty;
    public DateTime ReceiptDate { get; set; }
    public string BillNo { get; set; } = string.Empty;
    public string PatientNo { get; set; } = string.Empty;
    public string? PatientName { get; set; }
    public decimal AmountBilled { get; set; }
    public decimal AmountPaid { get; set; }
    public string PayType { get; set; } = string.Empty;
    public string? ReceivedBy { get; set; }
    public string? Remarks { get; set; }
    public string? ChequeNo { get; set; }
    public string? BankCode { get; set; }
    public DateTime? ValueDate { get; set; }
}
