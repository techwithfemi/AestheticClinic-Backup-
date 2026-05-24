using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PurchaseOrderDetail
{
    public long Idnum { get; set; }

    public string? Poid { get; set; }

    public string? ItemId { get; set; }

    public string? Category { get; set; }

    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? QtyUsed { get; set; }

    public double? QtyInStock { get; set; }

    public double? Qty { get; set; }

    public double? UnitPrice { get; set; }

    public double? UnitPriceLast { get; set; }

    public bool? IsApprv { get; set; }

    public bool? Suppres { get; set; }
}
