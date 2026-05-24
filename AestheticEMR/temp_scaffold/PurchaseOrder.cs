using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PurchaseOrder
{
    public string Poid { get; set; } = null!;

    public long? SupplierId { get; set; }

    public string? EmpId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ExpectedDate { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }
}
