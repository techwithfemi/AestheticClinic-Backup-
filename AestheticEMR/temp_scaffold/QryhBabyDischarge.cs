using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhBabyDischarge
{
    public DateTime BDate { get; set; }

    public DateTime? BTime { get; set; }

    public string? WardId { get; set; }

    public string PNo { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? Sex { get; set; }

    public DateTime? ProbDischDate { get; set; }

    public int? NoOfDays { get; set; }
}
