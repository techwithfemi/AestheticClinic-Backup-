using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockItemGen
{
    public string? DrgCode { get; set; }

    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string? QtyUnit { get; set; }

    public double? BulkUnit { get; set; }

    public double? PharmUnit { get; set; }

    public double UnitsInStock { get; set; }

    public double? UnitLevel { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    public string? Brand { get; set; }

    public double? Unit2 { get; set; }

    public double? Unit3 { get; set; }
}
