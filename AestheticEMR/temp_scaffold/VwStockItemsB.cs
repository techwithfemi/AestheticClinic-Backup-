using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockItemsB
{
    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string ItemCatId { get; set; } = null!;

    public string QuantityPerUnit { get; set; } = null!;

    public int UnitPrice { get; set; }

    public int UnitsInStock { get; set; }

    public int ReorderLevel { get; set; }

    public int Discontinued { get; set; }

    public string Brand { get; set; } = null!;
}
