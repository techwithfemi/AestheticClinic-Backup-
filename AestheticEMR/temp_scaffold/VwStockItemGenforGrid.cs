using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockItemGenforGrid
{
    public string StockItem { get; set; } = null!;

    public string Category { get; set; } = null!;

    public double BulkUnit { get; set; }

    public double PharmUnit { get; set; }

    public double UnitsInStock { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    public double? UnitLevel { get; set; }

    public double? Unit2 { get; set; }

    public double? Unit3 { get; set; }

    public string Remarks { get; set; } = null!;

    public string? QtyUnit { get; set; }
}
