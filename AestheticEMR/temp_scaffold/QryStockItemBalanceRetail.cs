using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockItemBalanceRetail
{
    public string? CategoryName { get; set; }

    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public double? ReorderLevel { get; set; }

    public double UnitsInStock { get; set; }
}
