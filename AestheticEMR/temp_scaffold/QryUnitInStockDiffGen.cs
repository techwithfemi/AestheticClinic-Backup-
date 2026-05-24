using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockDiffGen
{
    public string Category { get; set; } = null!;

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public double BulkUnit { get; set; }

    public double? ReOrderLevel { get; set; }

    public double UnitDiff { get; set; }
}
