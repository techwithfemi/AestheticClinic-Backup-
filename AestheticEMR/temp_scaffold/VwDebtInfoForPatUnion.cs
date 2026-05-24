using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDebtInfoForPatUnion
{
    public string PNo { get; set; } = null!;

    public double Debt { get; set; }

    public string? ClientCat { get; set; }
}
