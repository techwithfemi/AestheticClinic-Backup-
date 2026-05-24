using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhBillingReceiptPublic
{
    public DateTime ReceiptDate { get; set; }

    public DateTime? RTime { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string Fullname { get; set; } = null!;

    public string PaymentFor { get; set; } = null!;

    public string AmountInWord { get; set; } = null!;

    public string PayType { get; set; } = null!;

    public string? ClinicId { get; set; }

    public string? Receivedby { get; set; }

    public string PNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string CoyName { get; set; } = null!;
}
