using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockIssuedSumm
{
    public string? Poid { get; set; }

    public int QtyUsed { get; set; }

    public string ItemId { get; set; } = null!;

    public string? Category { get; set; }
}
