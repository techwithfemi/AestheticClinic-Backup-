using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDocMinNumOfPatOld1
{
    public bool? AttendedToByDoc { get; set; }

    public DateTime Date { get; set; }

    public string? Doctor { get; set; }

    public string ClinicId { get; set; } = null!;

    public string EmpId { get; set; } = null!;

    public int? NumOfPat { get; set; }
}
