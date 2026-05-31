using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class QryhBillingIncome
{
    public DateTime ReceiptDate { get; set; }

    public DateTime? RTime { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string Pno { get; set; } = null!;

    public string PaymentFor { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public string AmountInWord { get; set; } = null!;

    public decimal? Balance { get; set; }

    public string PayType { get; set; } = null!;

    public string Expr1 { get; set; } = null!;

    public string? ClinicId { get; set; }

    public string Fullname { get; set; } = null!;

    public string PatNo { get; set; } = null!;

    public string? Receivedby { get; set; }

    public string BillNo { get; set; } = null!;

    public string? Coyname { get; set; }

    public string? AcctId { get; set; }

    public bool? IsPost { get; set; }

    public string? Remarks { get; set; }

    public bool? Suppres { get; set; }
}
