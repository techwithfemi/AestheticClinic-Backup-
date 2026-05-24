using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhAttendanceAndDiagnosis
{
    public int RecId { get; set; }

    public DateTime RecDate { get; set; }

    public DateTime? Htime { get; set; }

    public string Status { get; set; } = null!;

    public string ConsultId { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? Symptoms { get; set; }

    public string Prescription { get; set; } = null!;

    public DateTime? NextApptDate { get; set; }

    public string? Complaints { get; set; }

    public string? Investigate { get; set; }

    public string? TreatedBy { get; set; }

    public string? Result { get; set; }

    public string Sex { get; set; } = null!;

    public string? CoyName { get; set; }

    public string? EmpId { get; set; }

    public string? Diagnosis { get; set; }

    public string? EmpNo { get; set; }

    public string? OldpNo { get; set; }

    public int? Age { get; set; }
}
