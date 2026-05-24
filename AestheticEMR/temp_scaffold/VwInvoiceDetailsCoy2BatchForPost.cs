using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetailsCoy2BatchForPost
{
    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string Company { get; set; } = null!;

    public string InvoiceNo { get; set; } = null!;

    public string? Debit { get; set; }

    public decimal? AmountInvoiced { get; set; }

    public string Posted { get; set; } = null!;

    public bool? IsPost { get; set; }

    public DateTime? BillDate { get; set; }

    public string Credit { get; set; } = null!;

    public string? BillMonth { get; set; }

    public string? BillYear { get; set; }
}
