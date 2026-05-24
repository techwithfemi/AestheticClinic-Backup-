using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAdmissionServiceForGrid
{
    public string Drug { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? QtyUnit { get; set; }

    public double? Private { get; set; }

    public double? Nhis { get; set; }

    public double? Hmo { get; set; }

    public double? Mthly { get; set; }

    public double? _3mthly { get; set; }

    public double? _6mthly { get; set; }

    public double? Cbn { get; set; }

    public double? Nepa { get; set; }
}
