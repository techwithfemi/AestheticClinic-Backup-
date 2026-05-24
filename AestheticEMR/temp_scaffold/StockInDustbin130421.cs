using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockInDustbin130421
{
    public long Sno { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ItemId { get; set; }

    public decimal? UnitsInStock { get; set; }

    public string? LocId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? BatchNo { get; set; }

    public bool? Suppres { get; set; }

    public string? ReceivedBy { get; set; }

    public string? Remarks { get; set; }

    public DateTime? PostDate { get; set; }

    public decimal? UnitCost { get; set; }

    public bool? IsPost { get; set; }

    public bool? Reversed { get; set; }

    public DateTime? EntryDate2 { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
