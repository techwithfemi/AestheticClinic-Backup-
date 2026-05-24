using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryReceiptCancelled
{
    public DateTime? ReceiptDate { get; set; }

    public DateTime? RTime { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? AmountInWord { get; set; }

    public decimal? Balance { get; set; }

    public string? PayType { get; set; }

    public string? PaymentFor { get; set; }
}
