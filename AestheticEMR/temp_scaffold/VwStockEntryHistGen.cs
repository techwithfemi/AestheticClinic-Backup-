using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockEntryHistGen
{
    public DateTime EntryDate { get; set; }

    public string ItemId { get; set; } = null!;

    public int Qty { get; set; }

    public decimal? Cost { get; set; }

    public string? Category { get; set; }
}
