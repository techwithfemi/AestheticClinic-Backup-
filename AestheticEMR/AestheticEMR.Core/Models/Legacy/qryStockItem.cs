using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockItem
{
    [StringLength(50)]
    public string ItemCode { get; set; } = null!;

    [StringLength(50)]
    public string ItemName { get; set; } = null!;

    [StringLength(50)]
    public string ItemCategory { get; set; } = null!;

    [Column("Qty/Unit")]
    [StringLength(50)]
    public string? Qty_Unit { get; set; }

    public double UnitPrice { get; set; }

    public double UnitsInStock { get; set; }

    public double? ReorderLevel { get; set; }
}
