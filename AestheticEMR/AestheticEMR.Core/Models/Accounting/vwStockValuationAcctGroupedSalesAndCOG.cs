using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwStockValuationAcctGroupedSalesAndCOG
{
    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public decimal? AmtOpBal { get; set; }

    public decimal? AmtPurch { get; set; }

    public decimal? AmtAvailBal { get; set; }

    public decimal? COGS { get; set; }

    public decimal? AmtClBal { get; set; }

    public decimal? StockSales { get; set; }

    public decimal? Profit { get; set; }

    public string? PeriodVal { get; set; }
}
