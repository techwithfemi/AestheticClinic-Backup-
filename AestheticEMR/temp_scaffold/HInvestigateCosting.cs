using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInvestigateCosting
{
    public long Sno { get; set; }

    public long LabItemSno { get; set; }

    public long StockItemSno { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Subtotal { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
