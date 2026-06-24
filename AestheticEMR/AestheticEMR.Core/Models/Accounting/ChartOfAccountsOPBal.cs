using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class ChartOfAccountsOPBal
{
    public long SNo { get; set; }

    public string AccountID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public DateTime TranDate { get; set; }

    public decimal OPAmt { get; set; }

    public decimal CLAmt { get; set; }

    public bool isClose { get; set; }

    public string Description { get; set; } = null!;

    public short SNo2 { get; set; }
}
