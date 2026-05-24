using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugsInsertBulk
{
    public string DrgName { get; set; } = null!;

    public string? PharmName { get; set; }

    public string? DrgCatName { get; set; }

    public string? QtyUnit { get; set; }

    public double? BulkUnit { get; set; }

    public string? StdPresc { get; set; }

    public double? StdQty { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    public string? Brand { get; set; }

    public string? DrgCode { get; set; }

    public string? Dept { get; set; }

    public string LocId { get; set; } = null!;
}
