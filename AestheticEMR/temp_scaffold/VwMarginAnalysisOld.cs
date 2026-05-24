using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwMarginAnalysisOld
{
    public DateTime BillDate { get; set; }

    public string? Mth { get; set; }

    public int? Yr { get; set; }

    public double Amount { get; set; }

    public string? Period { get; set; }

    public string? PeriodVal { get; set; }
}
