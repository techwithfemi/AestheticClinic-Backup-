using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwProfitAndLossDetail
{
    public decimal? DirectIncome { get; set; }

    public decimal? DirectCost { get; set; }

    public decimal? GrossProfit { get; set; }

    public decimal? IndirectCost { get; set; }

    public decimal? NetOprProfit { get; set; }

    public decimal? IndirectIncome { get; set; }

    public decimal? Taxation { get; set; }

    public decimal? ProfitBeforeTax { get; set; }

    public decimal? NetProfit { get; set; }

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public int? Yr { get; set; }

    public int? MonthCounter { get; set; }

    public string? PeriodVal { get; set; }
}
