using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhBillingReceiptRpt
{
    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal? Balance { get; set; }

    public string? Company { get; set; }

    public string PaymentFor { get; set; } = null!;

    public string AmountInWord { get; set; } = null!;

    public string PayType { get; set; } = null!;

    public string? ClinicId { get; set; }

    public string? Receivedby { get; set; }

    public string BillNo { get; set; } = null!;
}
