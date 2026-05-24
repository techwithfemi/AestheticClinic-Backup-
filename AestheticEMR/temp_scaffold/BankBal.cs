using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class BankBal
{
    public DateTime? EndDate { get; set; }

    public string? AcctId { get; set; }

    public double? CloseBal { get; set; }
}
