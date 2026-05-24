using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDebtInfoForPat
{
    public string? ClientCat { get; set; }

    public string PNo { get; set; } = null!;

    public decimal? Debt { get; set; }
}
