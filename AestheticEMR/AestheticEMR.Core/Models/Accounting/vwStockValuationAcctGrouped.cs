using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwStockValuationAcctGrouped
{
    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public decimal? AmtOpBal { get; set; }

    public decimal? AmtPurch { get; set; }

    public decimal? AmtAvailBal { get; set; }

    public string? PeriodVal { get; set; }
}
