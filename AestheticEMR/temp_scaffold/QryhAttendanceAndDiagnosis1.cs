using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhAttendanceAndDiagnosis1
{
    public DateTime RecDate { get; set; }

    public DateTime? Htime { get; set; }

    public string Fullname { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Prescription { get; set; } = null!;

    public string? LabTest { get; set; }

    public string Sex { get; set; } = null!;

    public string? Diagnosis { get; set; }

    public string? Nhisno { get; set; }

    public string? CardNo { get; set; }

    public int? Age { get; set; }
}
