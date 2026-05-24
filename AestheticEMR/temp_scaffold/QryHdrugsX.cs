using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryHdrugsX
{
    public string Drug { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? QtyUnit { get; set; }

    public double Cost { get; set; }

    public double? Nhis { get; set; }

    public double? Price { get; set; }

    public string Remarks { get; set; } = null!;

    public string? DeptBillCenter { get; set; }

    public string? StdPresc { get; set; }

    public double? StdQty { get; set; }

    public double? UnitsInStock { get; set; }
}
