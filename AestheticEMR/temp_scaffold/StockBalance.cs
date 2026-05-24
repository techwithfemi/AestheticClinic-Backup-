using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockBalance
{
    public long Sno { get; set; }

    public DateTime EntryDate { get; set; }

    public string ItemId { get; set; } = null!;

    public string LocId { get; set; } = null!;

    public decimal OpenBal { get; set; }

    public decimal? StockIn { get; set; }

    public decimal? StockOut { get; set; }

    public decimal? StockUsed { get; set; }

    public decimal? CloseBal { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
