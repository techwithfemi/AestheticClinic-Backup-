using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockEntryHist
{
    public DateTime? EntryDate { get; set; }

    public string? ItemId { get; set; }

    public double? Qty { get; set; }

    public decimal? Cost { get; set; }

    public string? Category { get; set; }
}
