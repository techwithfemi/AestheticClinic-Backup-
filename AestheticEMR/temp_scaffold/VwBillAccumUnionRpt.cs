using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillAccumUnionRpt
{
    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public string BillNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public decimal AmountBilled { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal? Balance { get; set; }

    public string PaymentFor { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? CoyName { get; set; }

    public string? OldpNo { get; set; }

    public string Issuedby { get; set; } = null!;
}
