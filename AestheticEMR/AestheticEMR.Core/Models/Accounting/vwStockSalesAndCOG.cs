using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwStockSalesAndCOG
{
    public long? ID { get; set; }

    public DateTime? EntryDate { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal Qty { get; set; }

    public decimal Price { get; set; }

    public decimal Cost { get; set; }

    public decimal? SubTotal { get; set; }

    public decimal? COGS { get; set; }

    public decimal? Sales { get; set; }

    public string? Period { get; set; }

    public string CoyID { get; set; } = null!;
}
