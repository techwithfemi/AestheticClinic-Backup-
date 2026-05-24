using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockItemBalance
{
    public string? CategoryName { get; set; }

    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public decimal? ReOrderLevel { get; set; }

    public decimal? UnitsInStock { get; set; }

    public string LocId { get; set; } = null!;

    public decimal? UnitCost { get; set; }

    public string? QtyPerUnit { get; set; }

    public decimal? Amount { get; set; }

    public string? DeptId { get; set; }
}
