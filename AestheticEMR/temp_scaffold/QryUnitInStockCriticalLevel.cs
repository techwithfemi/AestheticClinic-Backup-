using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUnitInStockCriticalLevel
{
    public string? ItemId { get; set; }

    public double? BulkUnit { get; set; }

    public string? ItemName { get; set; }

    public double? ReOrderLevel { get; set; }

    public double? CriticalLevel { get; set; }

    public string? LocId { get; set; }
}
