using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugInvAndServiceUnion
{
    public string? Service { get; set; }

    public double? Price { get; set; }

    public string? Company { get; set; }

    public string? RevType { get; set; }

    public string? Capitated { get; set; }
}
