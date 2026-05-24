using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetailsCoy2Batch
{
    public string? BillYear2 { get; set; }

    public string? BillMonth2 { get; set; }

    public string? BatchNo { get; set; }

    public string Company { get; set; } = null!;

    public string? CoyCode { get; set; }

    public decimal? AmountBilled { get; set; }

    public string InvNo { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public bool? Posted { get; set; }

    public string? BillYear { get; set; }

    public string? BillMonth { get; set; }

    public string? Period { get; set; }

    public string? AcctId { get; set; }
}
