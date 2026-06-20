using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

/// <summary>Request body for updating an existing receipt (PUT api/billing/receipts/{receiptNo}).</summary>
public class UpdateReceiptVM
{
    [Required]
    [StringLength(50)]
    public string PayType { get; set; } = string.Empty;

    public decimal? AmountBilled { get; set; }

    [StringLength(100)]
    public string? AccountNo { get; set; }

    [StringLength(50)]
    public string? ChequeNo { get; set; }

    [StringLength(20)]
    public string? BankCode { get; set; }

    public DateTime? ValueDate { get; set; }

    [StringLength(255)]
    public string? Remarks { get; set; }

    [StringLength(100)]
    public string? ReceivedBy { get; set; }
}
