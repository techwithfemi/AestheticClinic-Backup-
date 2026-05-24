using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhReferalForRpt
{
    public DateTime Date { get; set; }

    public long Id { get; set; }

    public string? PNo { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? ClientCat { get; set; }

    public DateTime? ApptDate { get; set; }

    public string? ClinicType { get; set; }

    public string? RefReason { get; set; }

    public DateTime? RefDate { get; set; }

    public string? Time { get; set; }

    public string? RefAddress { get; set; }

    public string? CoyId { get; set; }

    public bool? Suppres { get; set; }

    public string? EmpId { get; set; }

    public string? DocName { get; set; }

    public bool? AttendedToByRec { get; set; }

    public string CoyName { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? DiffDiagnosis { get; set; }

    public string? ConId { get; set; }

    public string Fullname { get; set; } = null!;

    public DateTime? RecDate { get; set; }

    public string Remarks { get; set; } = null!;
}
