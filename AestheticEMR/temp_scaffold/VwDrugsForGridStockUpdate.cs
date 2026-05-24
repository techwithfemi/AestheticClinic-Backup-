using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugsForGridStockUpdate
{
    public string Drug { get; set; } = null!;

    public string Category { get; set; } = null!;

    public double UnitsInStock { get; set; }

    public double? ReOrderLevel { get; set; }
}
