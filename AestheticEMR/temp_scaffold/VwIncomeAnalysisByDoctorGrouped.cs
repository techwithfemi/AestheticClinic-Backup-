using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwIncomeAnalysisByDoctorGrouped
{
    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? DocName { get; set; }

    public string? EmpId { get; set; }

    public string? Mth { get; set; }

    public string? Yr { get; set; }
}
