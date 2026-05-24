using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwMarginAnalysis
{
    public string? RetainId { get; set; }

    public int? Mth { get; set; }

    public int? Yr { get; set; }

    public double? Amount { get; set; }

    public string? Period { get; set; }

    public string? PeriodVal { get; set; }

    public string Company { get; set; } = null!;
}
