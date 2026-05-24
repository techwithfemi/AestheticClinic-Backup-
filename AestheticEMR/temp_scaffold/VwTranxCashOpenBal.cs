using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTranxCashOpenBal
{
    public long Sno { get; set; }

    public DateTime TranDate { get; set; }

    public string AcctId { get; set; } = null!;

    public double? Balance { get; set; }

    public DateTime? ValueDate { get; set; }
}
