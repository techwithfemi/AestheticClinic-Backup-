using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhConsultingPatListForLab
{
    public string ConsultId { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? Investigate { get; set; }

    public string PSurname { get; set; } = null!;

    public string? PFirstname { get; set; }

    public string? ClientCat { get; set; }

    public string Remarks { get; set; } = null!;

    public string? Treatedby { get; set; }

    public int? Age { get; set; }

    public string CoyCode { get; set; } = null!;

    public string? Coyname { get; set; }

    public string? Id { get; set; }

    public DateTime? CDate { get; set; }

    public DateTime? CTime { get; set; }

    public bool? Suppres { get; set; }

    public bool? AttendedtoByLab { get; set; }

    public string? Ref { get; set; }

    public string Company { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string RetainId { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public DateTime InvDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string? PolicyType { get; set; }

    public string? EmpNo { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis { get; set; }

    public string? Sex { get; set; }

    public string? PPhoneNo { get; set; }

    public string? Maturity { get; set; }

    public string? PhoneNo { get; set; }

    public string? Phone { get; set; }

    public string? LabNum { get; set; }
}
