using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockInDustbinAcct
{
    public long Sno { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Stock { get; set; }

    public decimal? UnitsInStock { get; set; }

    public string? LocId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool? Suppres { get; set; }

    public decimal? UnitCost { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public DateTime? EntryDate2 { get; set; }

    public bool? IsPost { get; set; }

    public bool? Reversed { get; set; }

    public decimal? Amount { get; set; }

    public string? AppName { get; set; }

    public string? Remarks { get; set; }
}
