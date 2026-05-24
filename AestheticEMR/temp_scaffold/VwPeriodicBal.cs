using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPeriodicBal
{
    public long Sno { get; set; }

    public string Period { get; set; } = null!;

    public string AcctId { get; set; } = null!;

    public string AcctName { get; set; } = null!;

    public double Balance { get; set; }

    public string? Remarks { get; set; }

    public string AcctType { get; set; } = null!;
}
