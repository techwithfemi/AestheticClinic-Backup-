using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwClinicDay
{
    public long Sno { get; set; }

    public int SnoId { get; set; }

    public string ClinicDay { get; set; } = null!;

    public string Clinic { get; set; } = null!;

    public DateTime ClinicTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int PatLimit { get; set; }
}
