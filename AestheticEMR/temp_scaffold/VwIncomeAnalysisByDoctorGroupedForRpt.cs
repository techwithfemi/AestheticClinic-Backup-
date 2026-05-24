using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwIncomeAnalysisByDoctorGroupedForRpt
{
    public string? EmpId { get; set; }

    public string? DocName { get; set; }

    public decimal Target { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }
}
