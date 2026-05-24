using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhVisitsStatsAttendance
{
    public DateTime RecDate { get; set; }

    public DateTime? Time { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string ClinicType { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? NextApptDate { get; set; }

    public int RecId { get; set; }

    public string? ClientCat { get; set; }

    public string? ClientCatId { get; set; }

    public string? EmpId { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? OldPno { get; set; }

    public string CoyName { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public string? PolicyType { get; set; }

    public string? CoyType { get; set; }

    public string? PCatId { get; set; }

    public string? Referal { get; set; }

    public string RetainName { get; set; } = null!;

    public string? EmpNo { get; set; }

    public string? Branch { get; set; }

    public string? Status { get; set; }

    public string? Area { get; set; }

    public string? LatestBillNo { get; set; }

    public string? PhoneNo { get; set; }

    public string? UserName { get; set; }

    public string? BioId { get; set; }

    public string? ClientCatId2 { get; set; }

    public string? Email { get; set; }

    public DateTime Date1 { get; set; }

    public string RetainId { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public DateTime? BillDate { get; set; }

    public DateTime? Dob { get; set; }

    public string? PPhoneNo { get; set; }

    public string? HmoRef { get; set; }

    public int? Age { get; set; }

    public int? AgeInMths { get; set; }

    public int? AgeInDays { get; set; }

    public DateTime Date { get; set; }

    public string? Sex { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatment { get; set; }

    public string? LabResult { get; set; }

    public string? DateAndTime { get; set; }
}
