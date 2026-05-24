using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwCashBankDailyBal
{
    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public long Sno { get; set; }

    public string AcctId { get; set; } = null!;

    public string AcctName { get; set; } = null!;

    public double Balance { get; set; }

    public string AcctType { get; set; } = null!;
}
