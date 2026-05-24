using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwIncomeAnalysisByClinicGrouped
{
    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string Clinic { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public long ClinicId { get; set; }
}
