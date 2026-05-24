using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPorderDetailsApprvGen
{
    public long Sno { get; set; }

    public string OrderNo { get; set; } = null!;

    public string? StockItem { get; set; }

    public string? Category { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public double? Amount { get; set; }

    public int? Qty { get; set; }

    public decimal? Cost { get; set; }

    public double? QtyInStock { get; set; }

    public long? ApprvId { get; set; }

    public long SnoPo { get; set; }

    public bool? AttendedTo { get; set; }
}
