using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockEntryGen
{
    public DateTime EntryDate { get; set; }

    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public int Qty { get; set; }

    public decimal? Cost { get; set; }

    public string? Comments { get; set; }

    public string? ReceivedBy { get; set; }

    public string? Category { get; set; }
}
