using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTrialBalanceByPeriod
{
    public DateTime TranDate { get; set; }

    public decimal? Amount { get; set; }

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? CoyID { get; set; }

    public string Period { get; set; } = null!;

    public string Expr1 { get; set; } = null!;
}
