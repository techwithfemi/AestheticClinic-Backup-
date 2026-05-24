using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAdmissionForCoy
{
    public long Id { get; set; }

    public DateTime CDate { get; set; }

    public DateTime? CTime { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string Prescription { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? DiffDiagnosis { get; set; }

    public string? Investigate { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Sex { get; set; }

    public string CoyName { get; set; } = null!;

    public string? OldPno { get; set; }

    public string? Remarks { get; set; }

    public DateTime AdmDate { get; set; }

    public DateTime? DischDate { get; set; }

    public DateTime? DischTime { get; set; }

    public int? NoOfDays { get; set; }

    public string? RetainCode { get; set; }
}
