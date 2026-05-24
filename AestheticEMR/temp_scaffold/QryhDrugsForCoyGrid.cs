using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhDrugsForCoyGrid
{
    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string? DrgCatGroup { get; set; }

    public string? QtyPerUnit { get; set; }

    public double Cost { get; set; }

    public string Remarks { get; set; } = null!;

    public double? Nhiscost { get; set; }
}
