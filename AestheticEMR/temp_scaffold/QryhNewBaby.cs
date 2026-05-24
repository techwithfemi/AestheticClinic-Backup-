using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhNewBaby
{
    public DateTime BDate { get; set; }

    public DateTime? BTime { get; set; }

    public string? WardId { get; set; }

    public string? Sex { get; set; }

    public string? Weight { get; set; }

    public string? Height { get; set; }

    public string Fullname { get; set; } = null!;
}
