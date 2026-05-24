using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhConsultingDetailsForBillingAlt
{
    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? Prescription { get; set; }

    public string? Treatedby { get; set; }

    public int? Age { get; set; }

    public string Company { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? Referal { get; set; }

    public string? Investigate { get; set; }

    public bool? AttendedTo { get; set; }

    public string? Injprescription { get; set; }

    public string? BillRemarks { get; set; }

    public string? Services { get; set; }

    public string RetainName { get; set; } = null!;

    public bool? IsDrug { get; set; }

    public bool? IsLab { get; set; }

    public bool? IsServ { get; set; }

    public string Remarks { get; set; } = null!;

    public string? Ref { get; set; }

    public string RetainId { get; set; } = null!;

    public double? Debt { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public bool? AttendedToByPharm { get; set; }

    public long? Id { get; set; }

    public DateTime? CDate { get; set; }

    public string? CTime { get; set; }

    public bool? Suppres { get; set; }

    public string? PatNo { get; set; }

    public string? Diagnosis { get; set; }

    public string? DiffDiagnosis { get; set; }

    public string RetainCode { get; set; } = null!;

    public string? ClientCatId { get; set; }

    public string? Investigate2 { get; set; }

    public string? Purpose { get; set; }

    public string? EmpNo { get; set; }

    public string? PolicyType { get; set; }

    public string ClinicType { get; set; } = null!;
}
