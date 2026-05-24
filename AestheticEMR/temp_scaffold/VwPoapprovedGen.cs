using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwPoapprovedGen
{
    public long Sno { get; set; }

    public long Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public string OrderNo { get; set; } = null!;

    public string? Drug { get; set; }

    public string? Category { get; set; }

    public double? QtyInStock { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public double? Amount { get; set; }

    public long SnoPo { get; set; }

    public bool? AttendedTo { get; set; }

    public string SupplierName { get; set; } = null!;

    public string Poid { get; set; } = null!;

    public string? ItemName { get; set; }

    public string Address { get; set; } = null!;

    public long? ApprvId { get; set; }
}
