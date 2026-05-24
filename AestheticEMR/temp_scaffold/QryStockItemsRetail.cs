using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockItemsRetail
{
    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string ItemCategory { get; set; } = null!;

    public string? QtyUnit { get; set; }

    public double? UnitPrice { get; set; }

    public double? UnitsInStock { get; set; }

    public double? ReOrderLevel { get; set; }

    public string? CatRemarks { get; set; }

    public string? DeptBillCenter { get; set; }

    public string? StdPresc { get; set; }

    public double? StdQty { get; set; }
}
