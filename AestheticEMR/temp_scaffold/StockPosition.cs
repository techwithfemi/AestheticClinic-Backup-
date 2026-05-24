using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockPosition
{
    public long Sno { get; set; }

    public DateTime OpenBalDate { get; set; }

    public string? ItemId { get; set; }

    public decimal Qty { get; set; }

    public decimal Cost { get; set; }

    public string LocId { get; set; } = null!;

    public decimal OpenBal { get; set; }

    public decimal? RunningTotalTest { get; set; }

    public string? Remarks { get; set; }

    public string? Drgcode { get; set; }
}
