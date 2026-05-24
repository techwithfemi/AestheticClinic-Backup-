using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwCapAndEnpense
{
    public string? CoyName { get; set; }

    public string? Mth { get; set; }

    public int? Yr { get; set; }

    public double? Amount { get; set; }

    public string HmoClass { get; set; } = null!;

    public string Remarks { get; set; } = null!;
}
