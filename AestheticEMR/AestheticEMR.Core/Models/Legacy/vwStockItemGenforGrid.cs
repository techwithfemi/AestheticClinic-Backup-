using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockItemGenforGrid
{
    [StringLength(350)]
    public string StockItem { get; set; } = null!;

    [StringLength(150)]
    public string Category { get; set; } = null!;

    public double BulkUnit { get; set; }

    public double PharmUnit { get; set; }

    public double UnitsInStock { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    public double? UnitLevel { get; set; }

    public double? Unit2 { get; set; }

    public double? Unit3 { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }
}
