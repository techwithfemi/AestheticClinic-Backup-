using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPatTimeline
{
    public int Sno { get; set; }

    public DateTime Date { get; set; }

    public string Fullname { get; set; } = null!;

    public string Company { get; set; } = null!;

    public DateTime? Attendance { get; set; }

    public string? Vitals { get; set; }

    public string? Consulting { get; set; }

    public string? Pharmacy { get; set; }

    public string? Remarks { get; set; }

    public string ConsultId { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? Prescription { get; set; }

    public string? Investigate { get; set; }

    public string? Services { get; set; }

    public decimal? AmountGen { get; set; }

    public decimal? AmountBilled { get; set; }

    public decimal? AmountPaid { get; set; }

    public string? Bill { get; set; }

    public string? Inv { get; set; }

    public DateTime? ApptDate { get; set; }

    public string? ApptTime { get; set; }

    public string? ApptClinic { get; set; }

    public string? ApptRemarks { get; set; }

    public string PNo { get; set; } = null!;

    public string? ClientCatId { get; set; }
}
