using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwProfitOrLossClosePrd
{
    public decimal? Amount { get; set; }

    public string CoyID { get; set; } = null!;
}
