using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwLabItemsForGrid
{
    public string LabItem { get; set; } = null!;

    public string? Category { get; set; }

    public int QtyPerUnit { get; set; }

    public string? Capitated { get; set; }

    public string? RangeVal { get; set; }

    public string? TestUnit { get; set; }

    public string? Class { get; set; }

    public string? Range { get; set; }

    public string? Sample { get; set; }

    public string RevenueType { get; set; } = null!;

    public string? ResultTemplate { get; set; }

    public string SubClass { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string? StockItem { get; set; }

    public string LabType { get; set; } = null!;
}
