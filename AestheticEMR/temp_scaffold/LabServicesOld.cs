using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class LabServicesOld
{
    public string? LabId { get; set; }

    public string DrgName { get; set; } = null!;

    public string? DrgCatName { get; set; }

    public string? QtyPerUnit { get; set; }

    public decimal? Private { get; set; }

    public double? Nhis { get; set; }

    public double? Hmo { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? Adc { get; set; }

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

    public string? RangeVal { get; set; }

    public string? TestUnit { get; set; }

    public string? Class { get; set; }

    public string? Range { get; set; }

    public string? Sample { get; set; }

    public string? RevType { get; set; }

    public string? ResultTemplate { get; set; }
}
