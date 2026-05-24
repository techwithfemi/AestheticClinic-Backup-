using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockItem
{
    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string ItemCategory { get; set; } = null!;

    public string? QtyUnit { get; set; }

    public double UnitPrice { get; set; }

    public double UnitsInStock { get; set; }

    public double? ReorderLevel { get; set; }
}
