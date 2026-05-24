using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhAdmissionHist
{
    public long Sno { get; set; }

    public DateTime AdmDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string? WardId { get; set; }

    public string? Reason { get; set; }

    public string? Remarks { get; set; }

    public string PNo { get; set; } = null!;

    public DateTime? ATime { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? ClientCat { get; set; }

    public string? Coyname { get; set; }

    public bool? IsDischarged { get; set; }

    public DateTime? Dob { get; set; }

    public int? Age { get; set; }

    public string? Sex { get; set; }

    public string? PolicyType { get; set; }

    public string? EmpNo { get; set; }

    public string Company { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public DateTime? Expr1 { get; set; }

    public int? Expr2 { get; set; }

    public string? Title { get; set; }

    public string? AdmitBy { get; set; }

    public string? AdmitedBy { get; set; }

    public string? DoctorName { get; set; }

    public string RetainCode { get; set; } = null!;

    public int? NoOfDays { get; set; }

    public string? AdmitingDoc { get; set; }
}
