using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOrderDetailsApprvGen
{
    public long SNO { get; set; }

    [StringLength(50)]
    public string OrderNo { get; set; } = null!;

    [StringLength(50)]
    public string? StockItem { get; set; }

    [StringLength(50)]
    public string? Category { get; set; }

    public double? QtyApprv { get; set; }

    public double? UnitPrice { get; set; }

    public double? Amount { get; set; }

    public int? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    public double? QtyInStock { get; set; }

    public long? ApprvID { get; set; }

    public long SnoPO { get; set; }

    public bool? AttendedTo { get; set; }
}
