using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockInDustbin
{
    public long Sno { get; set; }

    public string? StockItem { get; set; }

    public decimal? Qty { get; set; }

    public string? LocId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? BatchNo { get; set; }

    public bool? Suppres { get; set; }

    public string ReceivedBy { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Location { get; set; }

    public decimal? UnitCost { get; set; }

    public DateTime? PostDate { get; set; }

    public string? DeptId { get; set; }
}
