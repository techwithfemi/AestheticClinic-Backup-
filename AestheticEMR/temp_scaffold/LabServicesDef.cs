using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class LabServicesDef
{
    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string? QtyPerUnit { get; set; }

    public double? Nhis { get; set; }

    public double? Hmo { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? Adc { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? Private { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? _3mthly { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? _6mthly { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? Cbn { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? Nepa { get; set; }

    public string? Capitated { get; set; }
}
