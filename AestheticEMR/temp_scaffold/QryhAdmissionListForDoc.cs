using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhAdmissionListForDoc
{
    public long Sno { get; set; }

    public DateTime AdmDate { get; set; }

    public string Fullname { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string? Coyname { get; set; }

    public string Company { get; set; } = null!;

    public string? AdmitBy { get; set; }

    public string? AdmitedBy { get; set; }

    public bool? IsDischargedByDoc { get; set; }

    public string? Reason { get; set; }

    public DateTime? ATime { get; set; }

    public string? Remarks { get; set; }

    public string? WardId { get; set; }

    public int? Age { get; set; }

    public string? Sex { get; set; }

    public string? ClientCat { get; set; }
}
