using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwConsFreq2
{
    public string PNo { get; set; } = null!;

    public int? ConsFreq { get; set; }

    public decimal? AmountBilled { get; set; }

    public double Capitation { get; set; }

    public string? AttndMth { get; set; }

    public string? AttndYr { get; set; }

    public string Period { get; set; } = null!;

    public string? ClientCat { get; set; }
}
