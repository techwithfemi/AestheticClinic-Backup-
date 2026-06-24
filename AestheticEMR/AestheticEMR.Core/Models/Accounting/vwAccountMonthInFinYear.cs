using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwAccountMonthInFinYear
{
    public DateTime AcctMonth { get; set; }

    public int MonthCounter { get; set; }

    public string PeriodYr { get; set; } = null!;

    public string? Period { get; set; }

    public string? PeriodVal { get; set; }

    public bool isClose { get; set; }

    public DateTime PrdClose { get; set; }

    public string CoyID { get; set; } = null!;

    public bool? Suppres { get; set; }

    public string? PrdType { get; set; }

    public int? PrdStart { get; set; }

    public bool? Expr1 { get; set; }

    public int? diffVal { get; set; }

    public bool? expired { get; set; }
}
