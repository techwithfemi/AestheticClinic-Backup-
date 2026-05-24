using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwClinicDaysForRpt
{
    public string ClinicDay { get; set; } = null!;

    public string? Clinic { get; set; }

    public DateTime? ClinicTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? PatLimit { get; set; }

    public long? Sno { get; set; }
}
