using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingDiagnosisHmoOnline
{
    public long Id { get; set; }

    public DateTime CDate { get; set; }

    public DateTime? CTime { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? WardId { get; set; }

    public string? Symptoms { get; set; }

    public string Prescription { get; set; } = null!;

    public DateTime? NextApptDate { get; set; }

    public string? Preconsult { get; set; }

    public string? Complaints { get; set; }

    public string? SysReview { get; set; }

    public string? PhyExam { get; set; }

    public string? Investigate { get; set; }

    public string? Referto { get; set; }

    public string Fullname { get; set; } = null!;

    public string? TreatedBy { get; set; }

    public string? Result { get; set; }

    public int? Age { get; set; }

    public string? Sex { get; set; }

    public string? CoyName { get; set; }

    public string? PolicyType { get; set; }

    public string? EmpNo { get; set; }

    public string Company { get; set; } = null!;

    public string? GenSys { get; set; }

    public string? GenPhy { get; set; }

    public string? TreatPlan { get; set; }

    public string? EmpId { get; set; }

    public string? OldPno { get; set; }

    public string? Remarks { get; set; }

    public string? Diagnosis { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string RetainName { get; set; } = null!;

    public string PatCat { get; set; } = null!;

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }
}
