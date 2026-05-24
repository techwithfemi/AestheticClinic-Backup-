using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockPositionInMainStore
{
    public DateTime Date { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal? Store { get; set; }

    public decimal? Pharmacy { get; set; }

    public decimal? Issue { get; set; }

    public decimal? Entry { get; set; }
}
