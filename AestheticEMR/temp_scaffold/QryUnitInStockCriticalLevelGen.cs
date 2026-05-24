using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockCriticalLevelGen
{
    public string ItemId { get; set; } = null!;

    public double? BulkUnit { get; set; }

    public string ItemName { get; set; } = null!;

    public double? ReOrderLevel { get; set; }

    public double? CriticalLevel { get; set; }
}
