using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockEntryForGrid
{
    public long EntryId { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Category { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? QtyEntered { get; set; }

    public decimal? QtyInStockAsAtEntry { get; set; }

    public decimal? Total { get; set; }

    public decimal? PrevQtyUsed { get; set; }

    public decimal? UnitsInStore { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public decimal? QtyLastPurch { get; set; }

    public decimal? UnitPriceLast { get; set; }

    public string? BatchNo { get; set; }

    public string? Comments { get; set; }

    public string? Supplier { get; set; }

    public string? ReceivedBy { get; set; }

    public string? EmpId { get; set; }

    public string? ReverseId { get; set; }

    public string? Reversal { get; set; }
}
