using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockRegularLevelGen
{
    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public double? BulkUnit { get; set; }

    public double? ReOrderLevel { get; set; }

    public double? RegularLevel { get; set; }
}
