using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwIncomeAnalysisByClinicGroupedForRpt
{
    public string ClinicType { get; set; } = null!;

    public decimal Target { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }
}
