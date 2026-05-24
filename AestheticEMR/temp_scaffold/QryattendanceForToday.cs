using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryattendanceForToday
{
    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public string Fullname { get; set; } = null!;

    public string PNo { get; set; } = null!;

    public string? OldpNo { get; set; }

    public string ClinicType { get; set; } = null!;

    public DateTime? NextApptDate { get; set; }

    public string? Remarks { get; set; }
}
