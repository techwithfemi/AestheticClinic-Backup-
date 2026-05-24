using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class XxxxvwInvoiceDetailsCoyAcctPost
{
    public DateTime Date { get; set; }

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;

    public double? AmountBilled { get; set; }

    public string? InvNo { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public double AmountInvoiced { get; set; }

    public DateTime? BillDate { get; set; }

    public bool? IsPost { get; set; }

    public string? AcctId { get; set; }

    public string InvoiceNo { get; set; } = null!;
}
