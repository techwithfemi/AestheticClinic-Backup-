using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockBalancePeriodEnd
{
    public long Sno { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal UnitsInStock { get; set; }

    public decimal UnitPrice { get; set; }

    public string Period { get; set; } = null!;

    public DateOnly EntryDate { get; set; }

    public string? LocId { get; set; }

    public decimal? UnitLevel { get; set; }
}
