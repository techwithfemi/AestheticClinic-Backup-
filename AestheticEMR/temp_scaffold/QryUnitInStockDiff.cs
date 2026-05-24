using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockDiff
{
    public string? ItemCode { get; set; }

    public string? ItemName { get; set; }

    public double BulkUnit { get; set; }

    public double? ReOrderLevel { get; set; }

    public double UnitDiff { get; set; }

    public string? LocId { get; set; }
}
