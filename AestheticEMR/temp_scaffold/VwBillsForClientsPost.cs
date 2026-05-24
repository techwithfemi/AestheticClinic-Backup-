using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillsForClientsPost
{
    public long Sno { get; set; }

    public string Company { get; set; } = null!;

    public string? CoyCode { get; set; }

    public decimal AmountBilled { get; set; }

    public decimal? AmountInvoiced { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public string? BatchNo { get; set; }

    public string? BatchVal { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public bool? IsPost { get; set; }

    public string Posted { get; set; } = null!;

    public string? AcctId { get; set; }

    public string? Remarks { get; set; }
}
