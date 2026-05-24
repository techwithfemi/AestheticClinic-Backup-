using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwClinicDaysForRptCrossTab
{
    public string? Clinic { get; set; }

    public DateTime? Monday { get; set; }

    public DateTime? Tuesday { get; set; }

    public DateTime? Wednesday { get; set; }

    public DateTime? Thursday { get; set; }

    public DateTime? Friday { get; set; }

    public DateTime? Saturday { get; set; }

    public DateTime? Sunday { get; set; }
}
