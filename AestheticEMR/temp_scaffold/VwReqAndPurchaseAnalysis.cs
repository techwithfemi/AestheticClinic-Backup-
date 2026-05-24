using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwReqAndPurchaseAnalysis
{
    public DateTime? OrderDate { get; set; }

    public string? OrderNo { get; set; }

    public string? ItemId { get; set; }

    public string? Category { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? UnitPriceLast { get; set; }

    public double? QtyUsed { get; set; }

    public double? QtyInStock { get; set; }

    public double? QtyRqst { get; set; }

    public double? UnitPriceRqst { get; set; }

    public DateTime? OrderDateApprv { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? SuppId { get; set; }

    public decimal? QtyPurch { get; set; }

    public decimal? PricePurch { get; set; }

    public double? AmountLastPurch { get; set; }

    public double? AmountRqst { get; set; }

    public double? AmountApprv { get; set; }

    public decimal? AmountPurch { get; set; }

    public long? EntryId { get; set; }

    public long? ApprvId { get; set; }

    public long? Idnum { get; set; }
}
