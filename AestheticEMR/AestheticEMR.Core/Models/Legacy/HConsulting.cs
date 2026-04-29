using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class HConsulting
{
    public long Id { get; set; } // pri key

    public DateTime CDate { get; set; } // default to current date

    public DateTime? CTime { get; set; } // default to current time

    public string TreatedBy { get; set; } = null!; // empid of doctor

    public string ConsultId { get; set; } = null!; // from ui

    public string PNo { get; set; } = null!;

    public string ClientCat { get; set; } = null!;

    public string? WardId { get; set; } = null;// 

    public string? Symptoms { get; set; }

    public string? Prescription { get; set; }

    public string? Services { get; set; }

    public DateTime? NextApptDate { get; set; } =null;

    public string? Preconsult { get; set; } = null;     

    public string? Complaints { get; set; } 

    public string? SysReview { get; set; } = null;

    public string? PhyExam { get; set; } = null;
    public string? Diagnosis { get; set; } 

    public string? DiffDiagnosis { get; set; } = null;

    public string? Investigate { get; set; } = null;        

    public string? Hpc { get; set; } = null;

    public string? Pmh { get; set; } = null;

    public string? DrugHx { get; set; } = null;

    public string? Referto { get; set; } = null;

    public bool? AttendedToByPharm { get; set; } = false;

    public string? Remarks { get; set; } = null;        

    public bool? IsAlarm { get; set; }=false;

    public bool? IsReview { get; set; }=false;

    public string? Informt { get; set; } = null;

    public bool? AttendedTo { get; set; }=false;

    public string? Injprescription { get; set; } = null;

    public string? GenSys { get; set; } = null;

    public string? GenPhy { get; set; } = null;

    public string? TreatPlan { get; set; } = null;

    public string? Treatdone { get; set; } = null;

    public string? DentHist { get; set; } = null;

    public string? ExtraOralExam { get; set; } = null;

    public string? IntraOralExam { get; set; } = null;

    public string? Clinic { get; set; } = null;

    public string? TreatType { get; set; } = null;

    public bool? Suppres { get; set; } = false;

    public string? BillRemarks { get; set; } = null;

    public string? ClinicRemarks { get; set; } = null;

    public bool? IsServ { get; set; } = false;

    public bool? IsDrug { get; set; } = false;

    public bool? IsLab { get; set; } = false;

    public bool? AttendedtoByLab { get; set; } = false;

    public bool? IsInj { get; set; } = false;

    public bool? IsDress { get; set; }

    public string? MedRpt { get; set; } = null;

    public bool IsLatest { get; set; }=false;

    public DateTime? EntryDate { get; set; } // default to current date

    public DateTime? EntryTime { get; set; } // default to current time

    public bool? AttendedToByHmo { get; set; } = false;


    public DateTime? EditTime { get; set; } // default to current time

    public DateTime? EditDate { get; set; } // default to current date

    public string? TreatplanEdit { get; set; }=null;

    public string? TreatplanBeforeEdit { get; set; }=string.Empty;

    public string? AppName { get; set; } = null;

    public string? ClientName { get; set; } = null;
}
