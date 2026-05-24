using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockEntryGen
{
    public long EntryId { get; set; }

    public long? ApprvId { get; set; }

    public DateTime EntryDate { get; set; }

    public string ItemId { get; set; } = null!;

    public string? Category { get; set; }

    public string? BatchNo { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int Qty { get; set; }

    public decimal? Cost { get; set; }

    public int? StockQtyOut { get; set; }

    public string? ReceivedBy { get; set; }

    public string? Poid { get; set; }

    public string? Comments { get; set; }

    public string? InvType { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public string? Supplier { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? UnitPriceLast { get; set; }

    public double? QtyUsed { get; set; }

    public double? QtyInStockAsAtEntry { get; set; }

    public double? UnitPrice { get; set; }

    public string? ReverseId { get; set; }

    public string? Reversal { get; set; }
}
