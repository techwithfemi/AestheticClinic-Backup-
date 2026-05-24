using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockPositionInMainStore2
{
    public DateTime Date { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal? OpeningBalance { get; set; }

    public decimal? Entry { get; set; }

    public decimal? Issue { get; set; }
}
