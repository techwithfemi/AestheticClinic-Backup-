using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwChartOfAccountsClosingPeriodsDue
{
    public string AccountID { get; set; } = null!;

    public decimal AccountClAmt { get; set; }

    public string CoyID { get; set; } = null!;

    public DateTime PrdClose { get; set; }

    public string? Period { get; set; }

    public string? PeriodVal { get; set; }

    public bool isClose { get; set; }
}
