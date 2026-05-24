using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockSalesAndCost
{
    public DateTime EntryDate { get; set; }

    public string DrgName { get; set; } = null!;

    public decimal? Qty { get; set; }

    public decimal? Price { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Cogs { get; set; }

    public decimal? Sales { get; set; }
}
