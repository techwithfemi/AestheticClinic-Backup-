using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class ClinicDay
{
    public long Sno { get; set; }

    public string ClinicId { get; set; } = null!;

    public string ClinicDay1 { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int NumOfPat { get; set; }

    public string? Remarks { get; set; }
}
