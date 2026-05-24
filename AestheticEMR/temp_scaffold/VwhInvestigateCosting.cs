using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhInvestigateCosting
{
    public long Sno { get; set; }

    public long LabItemSno { get; set; }

    public long StockItemSno { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Subtotal { get; set; }

    public string? Remarks { get; set; }

    public string LabItem { get; set; } = null!;

    public string StockName { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
