using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockEntryRetail
{
    public int EntryId { get; set; }

    public long StockEntryId { get; set; }

    public DateTime EntryDate { get; set; }

    public string ItemId { get; set; } = null!;

    public string? BatchNo { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int Qty { get; set; }

    public int? StockQtyOut { get; set; }

    public string? ReceivedBy { get; set; }

    public string? Poid { get; set; }

    public string? Comments { get; set; }

    public string? InvType { get; set; }

    public string? Category { get; set; }
}
