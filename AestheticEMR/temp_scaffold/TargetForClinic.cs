using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class TargetForClinic
{
    public long Sno { get; set; }

    public string Mth { get; set; } = null!;

    public string Yr { get; set; } = null!;

    public string TargetId { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal? Actual { get; set; }

    public bool IsMet { get; set; }

    public string? Remarks { get; set; }
}
