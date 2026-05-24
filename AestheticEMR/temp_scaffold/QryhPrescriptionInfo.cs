using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhPrescriptionInfo
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

    public string? ClientCat { get; set; }

    public string? Injprescription { get; set; }

    public long Id { get; set; }

    public DateTime? Andate { get; set; }

    public string? GestAge { get; set; }

    public string? Fundus { get; set; }

    public string? Presentation { get; set; }

    public string? Fh { get; set; }

    public string? Oedema { get; set; }

    public string? Hb { get; set; }

    public string? Pcv { get; set; }

    public string? Tt { get; set; }

    public string? Tca { get; set; }

    public string? Mo { get; set; }

    public string? TreatPlan { get; set; }

    public string? TreatType { get; set; }

    public string? Clinic { get; set; }

    public string? Informt { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? CTime { get; set; }

    public string? Services { get; set; }

    public string? BillRemarks { get; set; }

    public string? ClinicRemarks { get; set; }

    public string? UrineAlb { get; set; }

    public string? UrineSug { get; set; }

    public string? Bp { get; set; }

    public string? Wt { get; set; }

    public string? Remarks { get; set; }

    public string? Treatdone { get; set; }

    public string? DentHist { get; set; }

    public string? ExtraOralExam { get; set; }

    public string? IntraOralExam { get; set; }

    public string? MedRpt { get; set; }

    public int? Age { get; set; }

    public DateTime? Dob { get; set; }

    public int? AgeThen { get; set; }

    public string? RefReason { get; set; }

    public string? Comments { get; set; }

    public string? Referal { get; set; }

    public string Doctor { get; set; } = null!;

    public string? Treatment { get; set; }

    public string Company { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public string? DateAndTime { get; set; }

    public string? Result { get; set; }

    public string? Sex { get; set; }

    public string? PolicyType { get; set; }

    public string? EmpNo { get; set; }

    public string? Title { get; set; }

    public string? Occupation { get; set; }

    public string? BloodGroup { get; set; }

    public string? Genotype { get; set; }

    public string? Purpose { get; set; }
}
