using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class StockValuationAcct2
{
    public long SNO { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public decimal? OpenBal { get; set; }

    public decimal? StockPurch { get; set; }

    public decimal? CloseBal { get; set; }

    public decimal? StockReconcile { get; set; }

    public decimal? StockSales { get; set; }

    public decimal? COGS { get; set; }

    public decimal? COGS2 { get; set; }

    public decimal? CloseBal2 { get; set; }

    public decimal? StockAdjust { get; set; }

    public int? Mth { get; set; }

    public int? Yr { get; set; }

    public string? Period { get; set; }

    public string? Remarks { get; set; }

    public string? CoyID { get; set; }
}
