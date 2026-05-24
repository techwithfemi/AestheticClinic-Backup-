using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HServices20250501152510
{
    public string Service { get; set; } = null!;

    public string? Category { get; set; }

    public string? Type { get; set; }

    public double? Private { get; set; }

    public double? Nhis { get; set; }

    public double? Hmo { get; set; }

    public double? Mthly { get; set; }

    public double? _3mthly { get; set; }

    public double? _6mthly { get; set; }

    public double? Nepa { get; set; }

    public double? Cbn { get; set; }

    public string? Capitated { get; set; }

    public string? RevType { get; set; }

    public string? ServiceId { get; set; }

    public long Sno { get; set; }
}
