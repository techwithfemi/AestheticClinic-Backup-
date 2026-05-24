using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugsZeroPrice
{
    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public double UnitsInStock { get; set; }

    public string? StdPresc { get; set; }

    public double? StdQty { get; set; }

    public string? QtyUnit { get; set; }

    public string? Private { get; set; }

    public string? Nhis { get; set; }

    public string? Hmo { get; set; }

    public string? Gene { get; set; }

    public double? _3mthly { get; set; }

    public double? _6mthly { get; set; }

    public double? Cbn { get; set; }

    public double? Nepa { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }
}
