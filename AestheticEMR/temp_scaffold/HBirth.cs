using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HBirth
{
    public int Sno { get; set; }

    public DateTime BDate { get; set; }

    public DateTime? BTime { get; set; }

    public string? WardId { get; set; }

    public string? Supervisedby { get; set; }

    public string? Sex { get; set; }

    public string? Weight { get; set; }

    public string? Height { get; set; }

    public string? PNo { get; set; }

    public DateTime? ProbDischDate { get; set; }

    public string? Remarks { get; set; }
}
