using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockEntryWtAvg
{
    public string? ItemId { get; set; }

    public string? Category { get; set; }

    public double? WtAvg { get; set; }

    public string? LocId { get; set; }
}
