using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class PeriodEndBalance
{
    public long SNo { get; set; }

    public string AccountID { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public bool isClose { get; set; }

    public bool isTransfer { get; set; }

    public string? Remarks { get; set; }
}
