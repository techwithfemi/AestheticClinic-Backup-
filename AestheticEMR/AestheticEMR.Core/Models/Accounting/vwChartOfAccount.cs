using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwChartOfAccount
{
    public long SNo { get; set; }

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public string? AccountDesc { get; set; }

    public bool? Hidden { get; set; }

    public string? ExtID { get; set; }

    public string? ExtIDType { get; set; }

    public bool isContra { get; set; }

    public decimal Amount { get; set; }

    public decimal? AcctBal { get; set; }

    public bool? isClose { get; set; }

    public long SNoTran { get; set; }
}
