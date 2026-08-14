using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class PurchaseOrderDetail
{
    public long IDNum { get; set; }

    [StringLength(50)]
    public string? POID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(250)]
    public string? Category { get; set; }

    [Column(TypeName = "datetime")]
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
