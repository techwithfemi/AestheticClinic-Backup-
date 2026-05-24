using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockUnionLevel
{
    public string? ItemId { get; set; }

    public string? ItemName { get; set; }

    public double? ReorderLevel { get; set; }

    public double? StockLevel { get; set; }

    public string AlertStatus { get; set; } = null!;

    public string? LocId { get; set; }
}
