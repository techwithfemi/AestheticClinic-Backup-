using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPoapproved
{
    public long Sno { get; set; }

    public long Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public string OrderNo { get; set; } = null!;

    public string? Drug { get; set; }

    public string? Category { get; set; }

    public double? QtyInStock { get; set; }

    public decimal Qty { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public double? Amount { get; set; }

    public long? ApprvId { get; set; }

    public long SnoPo { get; set; }

    public bool? AttendedTo { get; set; }

    public string SupplierName { get; set; } = null!;

    public string Poid { get; set; } = null!;

    public string? ItemName { get; set; }

    public string Address { get; set; } = null!;

    public DateTime? ExpiryDate { get; set; }

    public decimal? LastQty { get; set; }

    public decimal? LastPrice { get; set; }

    public decimal? LastQtyInStock { get; set; }

    public DateTime? LastDatePurch { get; set; }

    public DateTime? EntryDate { get; set; }

    public decimal? LastQtyPurch { get; set; }

    public decimal? LastUnitPrice { get; set; }

    public string? LastPoid { get; set; }

    public decimal? QtyUsed { get; set; }

    public string? LocId { get; set; }
}
