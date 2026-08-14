using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("PurchaseOrderDetailsApprove")]
public partial class PurchaseOrderDetailsApprove
{
    public long SNO { get; set; }

    public long SnoPO { get; set; }

    [StringLength(50)]
    public string POID { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(550)]
    public string? Category { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public bool? AttendedTo { get; set; }
}
