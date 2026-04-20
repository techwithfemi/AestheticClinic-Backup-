using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class HConsulting
{
    public long Id { get; set; }

    public DateTime CDate { get; set; }

    public DateTime? CTime { get; set; }

    public string TreatedBy { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string? WardId { get; set; }

    public string? Symptoms { get; set; }

    public string? Prescription { get; set; }

    public string? Services { get; set; }

    public DateTime? NextApptDate { get; set; }

    public string? Preconsult { get; set; }

    public string? Complaints { get; set; }

    public string? SysReview { get; set; }

    public string? PhyExam { get; set; }

    public string? Diagnosis { get; set; }

    public string? DiffDiagnosis { get; set; }

    public string? Investigate { get; set; }

    public string? Hpc { get; set; }

    public string? Pmh { get; set; }

    public string? DrugHx { get; set; }

    public string? Referto { get; set; }

    public bool? AttendedToByPharm { get; set; }

    public string? Remarks { get; set; }

    public bool? IsAlarm { get; set; }

    public bool? IsReview { get; set; }

    public string? Informt { get; set; }

    public bool? AttendedTo { get; set; }

    public string? Injprescription { get; set; }

    public string? GenSys { get; set; }

    public string? GenPhy { get; set; }

    public string? TreatPlan { get; set; }

    public string? Treatdone { get; set; }

    public string? DentHist { get; set; }

    public string? ExtraOralExam { get; set; }

    public string? IntraOralExam { get; set; }

    public string? Clinic { get; set; }

    public string? TreatType { get; set; }

    public bool? Suppres { get; set; }

    public string? BillRemarks { get; set; }

    public string? ClinicRemarks { get; set; }

    public bool? IsServ { get; set; }

    public bool? IsDrug { get; set; }

    public bool? IsLab { get; set; }

    public bool? AttendedtoByLab { get; set; }

    public bool? IsInj { get; set; }

    public bool? IsDress { get; set; }

    public string? MedRpt { get; set; }

    public bool IsLatest { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public bool? AttendedToByHmo { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public DateTime? EditTime { get; set; }

    public DateTime? EditDate { get; set; }

    public string? TreatplanEdit { get; set; }

    public string? TreatplanBeforeEdit { get; set; }
}
