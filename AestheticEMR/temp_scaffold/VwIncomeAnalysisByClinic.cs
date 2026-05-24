using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwIncomeAnalysisByClinic
{
    public string MthName { get; set; } = null!;

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string ClinicId { get; set; } = null!;

    public string Clinic { get; set; } = null!;

    public long Sno { get; set; }

    public DateTime RecDate { get; set; }

    public DateTime? Time { get; set; }

    public string Fullname { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? ClientCat { get; set; }

    public string RetainName { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }
}
