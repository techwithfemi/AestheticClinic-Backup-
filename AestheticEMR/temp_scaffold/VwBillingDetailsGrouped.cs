using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingDetailsGrouped
{
    public string BillNo { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public string? BillType { get; set; }

    public double? Qty { get; set; }

    public decimal? SubTotal { get; set; }

    public double Price { get; set; }

    public string BillTo { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? RevType { get; set; }

    public string? Dosage { get; set; }

    public bool? Reversed { get; set; }
}
