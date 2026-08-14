using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOrderDetailsGen
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string? OrderNo { get; set; }

    [StringLength(50)]
    public string? Drug { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastPurchaseDate { get; set; }

    public double? QtyLastPurchased { get; set; }

    public double? LastUnitPrice { get; set; }

    public double? QtyUsed { get; set; }

    public double? QtyInStock { get; set; }

    public double? QtyNeeded { get; set; }

    public double? UnitPrice { get; set; }

    public double? Amount { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string IsApprv { get; set; } = null!;
}
