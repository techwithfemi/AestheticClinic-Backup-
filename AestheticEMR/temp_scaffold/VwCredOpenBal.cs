using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwCredOpenBal
{
    public long Sno { get; set; }

    public DateTime TranDate { get; set; }

    public string AcctId { get; set; } = null!;

    public double? Balance { get; set; }
}
