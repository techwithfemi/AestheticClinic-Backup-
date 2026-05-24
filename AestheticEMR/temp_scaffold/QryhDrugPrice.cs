using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDrugPrice
{
    public string? CatRemarks { get; set; }

    public string DrgCatName { get; set; } = null!;

    public string DrgName { get; set; } = null!;

    public string? QtyPerUnit { get; set; }

    public double Cost { get; set; }
}
