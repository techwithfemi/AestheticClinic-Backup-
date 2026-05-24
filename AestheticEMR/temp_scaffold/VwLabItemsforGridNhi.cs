using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwLabItemsforGridNhi
{
    public string LabItem { get; set; } = null!;

    public string Category { get; set; } = null!;

    public double? Price { get; set; }

    public string? Company { get; set; }

    public string? Remarks { get; set; }
}
