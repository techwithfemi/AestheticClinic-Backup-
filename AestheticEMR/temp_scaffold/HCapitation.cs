using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HCapitation
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public int Yr { get; set; }

    public string Mth { get; set; } = null!;

    public string RetainId { get; set; } = null!;

    public double Amount { get; set; }

    public string? Remarks { get; set; }
}
