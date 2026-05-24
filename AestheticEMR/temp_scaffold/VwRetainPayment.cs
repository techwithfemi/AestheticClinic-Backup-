using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwRetainPayment
{
    public string Company { get; set; } = null!;

    public decimal AmountDue { get; set; }

    public decimal AmountPaid { get; set; }

    public string AmountInWord { get; set; } = null!;

    public string? Receivedby { get; set; }

    public string PayType { get; set; } = null!;

    public string? ChequeNo { get; set; }

    public string? Remarks { get; set; }

    public DateTime ReceiptDate { get; set; }

    public string ReceiptNo { get; set; } = null!;
}
