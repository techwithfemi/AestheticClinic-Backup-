using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPatTimelineOld
{
    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public DateTime? Attendance { get; set; }

    public DateTime? Vitals { get; set; }

    public DateTime? Consulting { get; set; }

    public DateTime? Pharmacy { get; set; }

    public string? Remarks { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? Prescription { get; set; }

    public string? Investigate { get; set; }

    public string? Services { get; set; }

    public double? AmountGen { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public DateTime? Inv { get; set; }

    public DateTime? Bill { get; set; }

    public DateTime? ApptDate { get; set; }

    public DateTime? ApptTime { get; set; }

    public string? ApptClinic { get; set; }

    public string? ApptRemarks { get; set; }
}
