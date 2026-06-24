using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTotalEquity
{
    public decimal? Amount { get; set; }

    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;
}
