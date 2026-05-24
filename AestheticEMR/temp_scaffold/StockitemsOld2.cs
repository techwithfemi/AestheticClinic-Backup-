using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockitemsOld2
{
    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string ItemCatId { get; set; } = null!;

    public string? QuantityPerUnit { get; set; }

    public double UnitPrice { get; set; }

    public double UnitsInStock { get; set; }

    public double? ReorderLevel { get; set; }

    public bool? Discontinued { get; set; }

    public string? Brand { get; set; }
}
