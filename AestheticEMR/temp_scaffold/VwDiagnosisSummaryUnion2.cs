using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDiagnosisSummaryUnion2
{
    public DateTime? Date { get; set; }

    public string? MthName { get; set; }

    public string? Yr { get; set; }

    public int? Mth { get; set; }

    public string? Period { get; set; }

    public string Diagnosis { get; set; } = null!;

    public int RecVal { get; set; }

    public string? PeriodVal { get; set; }
}
