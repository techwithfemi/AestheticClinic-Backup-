using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBankBalance
{
    public long Sno { get; set; }

    public DateTime Date { get; set; }

    public DateTime? Time { get; set; }

    public string AcctNo { get; set; } = null!;

    public double Balance { get; set; }

    public string BankName { get; set; } = null!;
}
