using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class PurchaseOrderApproveGen
{
    public string Poid { get; set; } = null!;

    public string? EmpId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Remarks { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public long Id { get; set; }
}
