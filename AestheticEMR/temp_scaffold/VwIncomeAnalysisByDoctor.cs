using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwIncomeAnalysisByDoctor
{
    public string ClinicId { get; set; } = null!;

    public DateTime RecDate { get; set; }

    public DateTime? Time { get; set; }

    public string Fullname { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? ClientCat { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string CoyName { get; set; } = null!;

    public string? DocName { get; set; }

    public string? EmpId { get; set; }

    public int Target { get; set; }
}
