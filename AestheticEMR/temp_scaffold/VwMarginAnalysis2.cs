using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwMarginAnalysis2
{
    public int? Mth { get; set; }

    public int? Yr { get; set; }

    public string? Company { get; set; }

    public string? RetainId { get; set; }

    public double AmountCap { get; set; }

    public double AmountCost { get; set; }

    public double? Margin { get; set; }

    public string? Period { get; set; }

    public string? PeriodVal { get; set; }
}
