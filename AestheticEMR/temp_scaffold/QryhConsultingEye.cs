using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingEye
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

    public string? Diagnosis { get; set; }

    public string? DiffDiagnosis { get; set; }

    public string? Investigate { get; set; }

    public string? Referto { get; set; }

    public string? Fullname { get; set; }

    public string? TreatedBy { get; set; }

    public string? Result { get; set; }

    public int? Age { get; set; }

    public string Sex { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? PolicyType { get; set; }

    public string? Company { get; set; }

    public string? GenSys { get; set; }

    public string? GenPhy { get; set; }

    public string? TreatPlan { get; set; }

    public string? EmpId { get; set; }

    public string? OldpNo { get; set; }

    public string? CoyType { get; set; }

    public string? Remarks { get; set; }

    public string? Injprescription { get; set; }

    public string? Hpc { get; set; }

    public string? Pmh { get; set; }

    public string? DrugHx { get; set; }

    public string ClientCat { get; set; } = null!;

    public string? Informt { get; set; }

    public string? VisualAcuity { get; set; }

    public string? Aided { get; set; }

    public string? PrevSpecRx { get; set; }

    public string? SubjectiveRefraction { get; set; }

    public string? ExtExamOd { get; set; }

    public string? ExtExamOs { get; set; }

    public string? IntExamOd { get; set; }

    public string? IntExamOs { get; set; }

    public string? EyeRemarks { get; set; }
}
