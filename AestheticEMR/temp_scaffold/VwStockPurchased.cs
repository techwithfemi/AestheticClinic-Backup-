using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockPurchased
{
    public DateTime? EntryDate { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Amount { get; set; }

    public string? LocId { get; set; }
}
