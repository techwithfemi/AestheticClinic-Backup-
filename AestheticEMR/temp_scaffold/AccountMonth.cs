using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AccountMonth
{
    public DateTime? AcctMonth { get; set; }

    public int MonthCounter { get; set; }

    public string? PeriodYr { get; set; }
}
