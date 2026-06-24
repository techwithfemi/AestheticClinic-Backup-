using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwStockEntryForValuationAcctStore
{
    public long EntryID { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ItemName { get; set; }

    public decimal? PrevQty { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Cost { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? OPBal { get; set; }

    public decimal? AmtPurch { get; set; }

    public decimal? AvailBal { get; set; }

    public string? Period { get; set; }

    public string CoyID { get; set; } = null!;

    public string? LocID { get; set; }
}
