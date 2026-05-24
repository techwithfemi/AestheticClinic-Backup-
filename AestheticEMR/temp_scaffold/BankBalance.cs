using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BankBalance
{
    public long Sno { get; set; }

    public DateTime Bdate { get; set; }

    public DateTime? Btime { get; set; }

    public string AcctId { get; set; } = null!;

    public double Balance { get; set; }
}
