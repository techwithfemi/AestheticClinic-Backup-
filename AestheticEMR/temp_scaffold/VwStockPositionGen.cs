using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockPositionGen
{
    public long EntryId { get; set; }

    public DateTime EntryDate { get; set; }

    public string ItemId { get; set; } = null!;

    public string Drug { get; set; } = null!;

    public int Qty { get; set; }

    public decimal? Cost { get; set; }

    public string? ReceivedBy { get; set; }

    public string? Comments { get; set; }

    public double? PriceLast { get; set; }

    public bool? Suppres { get; set; }

    public double? QtyUsed { get; set; }

    public double? BulkUnit { get; set; }

    public double? ReOrderLevel { get; set; }

    public string? QtyUnit { get; set; }

    public string? BatchNo { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Category { get; set; }

    public string DrgCatName { get; set; } = null!;
}
