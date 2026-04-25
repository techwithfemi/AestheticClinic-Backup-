using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class hService
{
    public string Service { get; set; } = null!;

    public string? Category { get; set; }

    public string? TYPE { get; set; }

    public double? Private { get; set; }

    public double? NHIS { get; set; }

    public double? HMO { get; set; }

    public double? MTHLY { get; set; }

    public double? _3MTHLY { get; set; }

    public double? _6MTHLY { get; set; }

    public double? NEPA { get; set; }

    public double? CBN { get; set; }

    public string? Capitated { get; set; }

    public string? revType { get; set; }

    public string? ServiceID { get; set; }

    public long SNo { get; set; }
}
