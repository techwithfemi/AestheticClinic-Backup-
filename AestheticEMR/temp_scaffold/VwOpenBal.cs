using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwOpenBal
{
    public string AcctId { get; set; } = null!;

    public double OpenBal { get; set; }

    public string? Remarks { get; set; }
}
