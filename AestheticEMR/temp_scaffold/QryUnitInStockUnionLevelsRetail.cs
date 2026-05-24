using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockUnionLevelsRetail
{
    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public double? ReorderLevel { get; set; }

    public double StockLevel { get; set; }

    public string AlertStatus { get; set; } = null!;
}
