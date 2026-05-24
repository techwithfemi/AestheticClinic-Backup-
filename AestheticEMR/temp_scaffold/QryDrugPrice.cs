using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryDrugPrice
{
    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string? QtyPerUnit { get; set; }

    public double Cost { get; set; }

    public double? Price { get; set; }
}
