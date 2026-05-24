using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAttendanceSumm
{
    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? MonthName { get; set; }

    public int? Num { get; set; }

    public DateTime RecDate { get; set; }
}
