using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockEntry
{
    public DateTime? EntryDate { get; set; }

    public string? ItemId { get; set; }

    public string? ItemName { get; set; }

    public decimal? Qty { get; set; }

    public decimal? Cost { get; set; }

    public decimal? UnitPrice { get; set; }

    public string? Comments { get; set; }

    public string? ReceivedBy { get; set; }

    public string? Category { get; set; }

    public string? LocId { get; set; }

    public decimal? PrevBal { get; set; }

    public string? PrevPoid { get; set; }

    public string? QtyPerUnit { get; set; }

    public string? SupplierName { get; set; }

    public long EntryId { get; set; }

    public string? DeptId { get; set; }
}
