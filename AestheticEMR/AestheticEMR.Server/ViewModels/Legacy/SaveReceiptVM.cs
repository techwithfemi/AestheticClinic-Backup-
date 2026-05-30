using System.ComponentModel.DataAnnotations;

namespace AestheticEMR.Server.ViewModels.Legacy;

/// <summary>Request body for saving a receipt (POST api/billing/{billNo}/receipt).</summary>
public class SaveReceiptVM
{
    /// <summary>Payment method, e.g. Cash, Cheque, Transfer, POS.</summary>
    [Required]
    [StringLength(50)]
    public string PayType { get; set; } = string.Empty;

    /// <summary>Name/code of the revenue account to debit (defaults to first hRevenueType if omitted).</summary>
    [StringLength(100)]
    public string? AccountNo { get; set; }

    /// <summary>Cheque number (for Cheque payments).</summary>
    [StringLength(50)]
    public string? ChequeNo { get; set; }

    /// <summary>Bank code (for Cheque/Transfer payments).</summary>
    [StringLength(20)]
    public string? BankCode { get; set; }

    /// <summary>Value / clearing date (for Cheque/Transfer payments).</summary>
    public DateTime? ValueDate { get; set; }

    /// <summary>Optional free-text remarks.</summary>
    [StringLength(255)]
    public string? Remarks { get; set; }

    /// <summary>Name of the cashier collecting the payment (defaults to current user).</summary>
    [StringLength(100)]
    public string? ReceivedBy { get; set; }
}

/// <summary>Response returned after a receipt is successfully saved.</summary>
public class ReceiptSavedVM
{
    public string ReceiptNo { get; set; } = string.Empty;
    public DateTime ReceiptDate { get; set; }
    public decimal AmountPaid { get; set; }
    public string PayType { get; set; } = string.Empty;
}
