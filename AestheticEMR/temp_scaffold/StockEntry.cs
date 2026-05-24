using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockEntry
{
    public long EntryId { get; set; }

    public long? ApprvId { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ItemId { get; set; }

    public string? Category { get; set; }

    public string? SuppId { get; set; }

    public string? BatchNo { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Cost { get; set; }

    public double? StockQtyOut { get; set; }

    public string? ReceivedBy { get; set; }

    public string? Poid { get; set; }

    public string? Comments { get; set; }

    public string? InvType { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public string? Supplier { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public decimal? QtyLastPurch { get; set; }

    public decimal? UnitPriceLast { get; set; }

    public decimal? QtyUsed { get; set; }

    public decimal? QtyInStockAsAtEntry { get; set; }

    public decimal? UnitPrice { get; set; }

    public string? ReverseId { get; set; }

    public string? Reversal { get; set; }

    public string? LocId { get; set; }

    public decimal? PrevBal { get; set; }

    public string? PrevPoid { get; set; }

    public string? Drgcode { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public bool? IsPost { get; set; }

    public DateTime? EntryDate2 { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }
}
