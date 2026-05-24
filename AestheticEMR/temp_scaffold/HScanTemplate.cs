using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HScanTemplate
{
    public long Sno { get; set; }

    public string Category { get; set; } = null!;

    public string Details { get; set; } = null!;

    public string? InvType { get; set; }
}
