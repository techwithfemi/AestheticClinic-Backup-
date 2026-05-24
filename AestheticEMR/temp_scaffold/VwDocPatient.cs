using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocPatient
{
    public DateTime RecDate { get; set; }

    public DateTime? Time { get; set; }

    public string Fullname { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? DocName { get; set; }

    public string? EmpId { get; set; }

    public DateTime Date1 { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? Remarks { get; set; }

    public string? ClientCat { get; set; }
}
