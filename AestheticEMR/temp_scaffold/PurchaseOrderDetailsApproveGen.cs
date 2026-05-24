using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PurchaseOrderDetailsApproveGen
{
    public long Sno { get; set; }

    public long SnoPo { get; set; }

    public string Poid { get; set; } = null!;

    public string? ItemId { get; set; }

    public string? Category { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public bool? AttendedTo { get; set; }
}
