using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockMovt
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string DrgName { get; set; } = null!;

    public decimal Qty { get; set; }

    public string Remarks { get; set; } = null!;

    public decimal? UnitCost { get; set; }
}
