using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPrescriptionInfoForEye
{
    public string ConsultId { get; set; } = null!;

    public DateTime CDate { get; set; }

    public string PNo { get; set; } = null!;

    public string? Prescription { get; set; }

    public string TreatedBy { get; set; } = null!;

    public string? Preconsult { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Complaints { get; set; }

    public string? SysReview { get; set; }

    public string? PhyExam { get; set; }

    public string? Diagnosis { get; set; }

    public string? DiffDiagnosis { get; set; }

    public string? Investigate { get; set; }

    public string? Referto { get; set; }

    public string? Hpc { get; set; }

    public string? Pmh { get; set; }

    public string? DrugHx { get; set; }

    public bool? AttendedToByPharm { get; set; }

    public string ClientCat { get; set; } = null!;

    public string? Injprescription { get; set; }

    public long Id { get; set; }

    public string? Remarks { get; set; }

    public DateTime? CTime { get; set; }

    public string? GenSys { get; set; }

    public string? GenPhy { get; set; }

    public string? TreatPlan { get; set; }

    public string? Treatdone { get; set; }

    public string? DentHist { get; set; }

    public string? ExtraOralExam { get; set; }

    public string? IntraOralExam { get; set; }

    public string? Clinic { get; set; }

    public DateTime Tdate { get; set; }

    public DateTime Ttime { get; set; }

    public string? VisualAcuity { get; set; }

    public string? Aided { get; set; }

    public string? PrevSpecRx { get; set; }

    public string? SubjectiveRefraction { get; set; }

    public string? ExtExamOd { get; set; }

    public string? ExtExamOs { get; set; }

    public string? IntExamOd { get; set; }

    public string? IntExamOs { get; set; }

    public string? RemarksEye { get; set; }

    public string? ConId { get; set; }

    public string? Retino { get; set; }

    public string? Refraction { get; set; }

    public string? FsprescRe { get; set; }

    public string? FsprescLe { get; set; }

    public string? Tonometry { get; set; }
}
