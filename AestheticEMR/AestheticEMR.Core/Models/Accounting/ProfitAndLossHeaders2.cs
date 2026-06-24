using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class ProfitAndLossHeaders2
{
    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string? PeriodVal { get; set; }

    public string? RptType { get; set; }

    public decimal? DirectCost { get; set; }

    public decimal? DirectIncome { get; set; }

    public decimal? IndirectCost { get; set; }

    public decimal? IndirectIncome { get; set; }

    public decimal? InterestPayable { get; set; }

    public decimal? Taxation { get; set; }

    public decimal? All_SUM { get; set; }
}
