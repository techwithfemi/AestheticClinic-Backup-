using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDispensingForRptGrouped
{
    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public double? Qty { get; set; }

    public double? Price { get; set; }

    public double? Amount { get; set; }

    public double? Cost { get; set; }
}
