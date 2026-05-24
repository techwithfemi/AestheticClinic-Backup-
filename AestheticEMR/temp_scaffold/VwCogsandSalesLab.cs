using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwCogsandSalesLab
{
    public long Sno { get; set; }

    public DateTime InvDate { get; set; }

    public DateTime? EntryDate { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? SympItem { get; set; }

    public double? Qty { get; set; }

    public decimal? Cost { get; set; }

    public double? Price { get; set; }

    public double? CostAmount { get; set; }

    public double? SalesAmount { get; set; }

    public bool? IsPost { get; set; }

    public bool? Suppres { get; set; }

    public double? Margin { get; set; }

    public string? TranId { get; set; }

    public long? ReversedPair { get; set; }

    public string? Remarks { get; set; }

    public bool? Reversed { get; set; }
}
