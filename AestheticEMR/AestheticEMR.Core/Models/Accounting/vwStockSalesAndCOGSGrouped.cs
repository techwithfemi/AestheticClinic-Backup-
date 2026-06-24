using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwStockSalesAndCOGSGrouped
{
    public string CoyID { get; set; } = null!;

    public string? Period { get; set; }

    public decimal? COGS { get; set; }

    public decimal? Sales { get; set; }

    public decimal? Profit { get; set; }

    public string? PeriodVal { get; set; }
}
