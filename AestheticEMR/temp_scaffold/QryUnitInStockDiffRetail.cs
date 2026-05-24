using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockDiffRetail
{
    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public double UnitsInStock { get; set; }

    public double? ReorderLevel { get; set; }

    public double? UnitDiff { get; set; }
}
