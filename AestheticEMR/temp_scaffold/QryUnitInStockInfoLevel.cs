using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockInfoLevel
{
    public string? ItemId { get; set; }

    public string? ItemName { get; set; }

    public double? BulkUnit { get; set; }

    public double? ReOrderLevel { get; set; }

    public double? InfoLevel { get; set; }

    public string? LocId { get; set; }
}
