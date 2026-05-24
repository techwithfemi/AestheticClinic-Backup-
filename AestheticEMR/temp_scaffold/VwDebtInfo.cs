using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDebtInfo
{
    public long Sno { get; set; }

    public string? Pno { get; set; }

    public decimal Debt { get; set; }

    public string Remarks { get; set; } = null!;

    public string? RetainCode { get; set; }

    public string Company { get; set; } = null!;

    public string? FullName { get; set; }
}
