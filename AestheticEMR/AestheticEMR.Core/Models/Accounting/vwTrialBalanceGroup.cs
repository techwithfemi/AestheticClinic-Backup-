using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTrialBalanceGroup
{
    public string GroupID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public long GroupMin { get; set; }

    public long GroupMax { get; set; }

    public decimal? AccountOpAmt { get; set; }

    public decimal? AccountClAmt { get; set; }

    public string? AccountCat { get; set; }

    public string? Remarks { get; set; }

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;
}
