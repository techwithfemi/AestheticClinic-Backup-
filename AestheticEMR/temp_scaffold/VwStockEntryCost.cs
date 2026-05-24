using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockEntryCost
{
    public long EntryId { get; set; }

    public string? ItemId { get; set; }

    public string? Category { get; set; }

    public double? Qty { get; set; }

    public decimal? Cost { get; set; }

    public string? LocId { get; set; }
}
