using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockItemBalanceGen
{
    public string CategoryName { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public double? ReOrderLevel { get; set; }

    public double? UnitsInStock { get; set; }
}
