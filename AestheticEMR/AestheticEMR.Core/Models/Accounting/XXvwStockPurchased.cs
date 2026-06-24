using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class XXvwStockPurchased
{
    public DateTime? EntryDate { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Amount { get; set; }
}
