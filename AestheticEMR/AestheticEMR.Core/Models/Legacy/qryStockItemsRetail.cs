using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockItemsRetail
{
    [StringLength(250)]
    public string ItemCode { get; set; } = null!;

    [StringLength(250)]
    public string ItemName { get; set; } = null!;

    [StringLength(250)]
    public string ItemCategory { get; set; } = null!;

    [Column("Qty/Unit")]
    [StringLength(50)]
    public string? Qty_Unit { get; set; }

    public double? UnitPrice { get; set; }

    public double? UnitsInStock { get; set; }

    public double? ReOrderLevel { get; set; }

    [StringLength(150)]
    public string? catRemarks { get; set; }

    [StringLength(50)]
    public string? deptBillCenter { get; set; }

    [StringLength(2500)]
    public string? stdPresc { get; set; }

    public double? stdQty { get; set; }
}
