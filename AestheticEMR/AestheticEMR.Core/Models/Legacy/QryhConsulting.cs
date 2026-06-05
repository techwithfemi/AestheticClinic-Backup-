using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class QryhConsulting
{
    public long Id { get; set; }

    public DateTime CDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string? CTime { get; set; }

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

    public string? Referto { get; set; }

    public string? TreatedBy { get; set; }

    public string? Result { get; set; }

    public string? RetainId { get; set; }

    public string? PolicyType { get; set; }

    public string? GenSys { get; set; }

    public string? GenPhy { get; set; }

    public string? TreatPlan { get; set; }

    public string? OldpNo { get; set; }

    public string? Remarks { get; set; }

    public string? Injprescription { get; set; }

    public string? Hpc { get; set; }

    public string? Pmh { get; set; }

    public string? DrugHx { get; set; }

    public string ClientCat { get; set; } = null!;

    public string? Informt { get; set; }

    public string? Services { get; set; }

    public string? Investigate { get; set; }

    public string? CoyType { get; set; }

    public string? Sex { get; set; }

    public string? EmpId { get; set; }

    public string Company { get; set; } = null!;

    public int? Age { get; set; }

    public string? Treatment { get; set; }

    public string? EmpNo { get; set; }

    public string Client { get; set; } = null!;

    public string MedRpt { get; set; } = null!;

    public string Findings { get; set; } = null!;

    public string Prosedure { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public bool IsLatest { get; set; }

    public string RetainCode { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClinicRemarks { get; set; }

    public string? BillRemarks { get; set; }
}
