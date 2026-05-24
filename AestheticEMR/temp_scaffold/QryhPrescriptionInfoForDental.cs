using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPrescriptionInfoForDental
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

    public DateTime TDate { get; set; }

    public DateTime TTime { get; set; }

    public bool? Auli1 { get; set; }

    public bool? Auli2 { get; set; }

    public bool? Aulc { get; set; }

    public bool? Aulpm1 { get; set; }

    public bool? Aulpm2 { get; set; }

    public bool? Aulm1 { get; set; }

    public bool? Aulm2 { get; set; }

    public bool? Aulm3 { get; set; }

    public bool? Auri1 { get; set; }

    public bool? Auri2 { get; set; }

    public bool? Aurc { get; set; }

    public bool? Aurpm1 { get; set; }

    public bool? Aurpm2 { get; set; }

    public bool? Aurm1 { get; set; }

    public bool? Aurm2 { get; set; }

    public bool? Aurm3 { get; set; }

    public bool? Alli1 { get; set; }

    public bool? Alli2 { get; set; }

    public bool? Allc { get; set; }

    public bool? Allpm1 { get; set; }

    public bool? Allpm2 { get; set; }

    public bool? Allm1 { get; set; }

    public bool? Allm2 { get; set; }

    public bool? Allm3 { get; set; }

    public bool? Alri1 { get; set; }

    public bool? Alri2 { get; set; }

    public bool? Alrc { get; set; }

    public bool? Alrpm1 { get; set; }

    public bool? Alrpm2 { get; set; }

    public bool? Alrm1 { get; set; }

    public bool? Alrm2 { get; set; }

    public bool? Alrm3 { get; set; }

    public bool? Culi1 { get; set; }

    public bool? Culi2 { get; set; }

    public bool? Culc { get; set; }

    public bool? Culpm1 { get; set; }

    public bool? Culpm2 { get; set; }

    public bool? Curi1 { get; set; }

    public bool? Curi2 { get; set; }

    public bool? Curc { get; set; }

    public bool? Curpm1 { get; set; }

    public bool? Curpm2 { get; set; }

    public bool? Clli1 { get; set; }

    public bool? Clli2 { get; set; }

    public bool? Cllc { get; set; }

    public bool? Cllpm1 { get; set; }

    public bool? Cllpm2 { get; set; }

    public bool? Clri1 { get; set; }

    public bool? Clri2 { get; set; }

    public bool? Clrc { get; set; }

    public bool? Clrpm1 { get; set; }

    public bool? Clrpm2 { get; set; }

    public string? ARem { get; set; }

    public string? CRem { get; set; }

    public string? Dtype { get; set; }
}
