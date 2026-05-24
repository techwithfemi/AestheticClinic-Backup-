using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockPosition
{
    public long? EntryId { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ItemId { get; set; }

    public string? Drug { get; set; }

    public decimal? Qty { get; set; }

    public decimal? PriceLast { get; set; }

    public string? ReceivedBy { get; set; }

    public string? Comments { get; set; }

    public string? Category { get; set; }

    public double? DrugPriceLast { get; set; }

    public bool? Suppres { get; set; }

    public double? BulkUnit { get; set; }

    public string? LocId { get; set; }

    public string? Poid { get; set; }

    public string? BatchNo { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
