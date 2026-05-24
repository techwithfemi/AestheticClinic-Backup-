using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvoiceDetailsAndBillingDetailsDiffForReconcile
{
    public long Sno { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? RevType { get; set; }

    public string BillNo { get; set; } = null!;

    public DateTime AttndDate { get; set; }

    public DateTime? BillDate { get; set; }

    public string ServiceRev { get; set; } = null!;

    public decimal SubTotalRev { get; set; }

    public string ServiceInv { get; set; } = null!;

    public decimal? SubTotalInv { get; set; }

    public string RetainCode { get; set; } = null!;

    public string CoyCode { get; set; } = null!;
}
