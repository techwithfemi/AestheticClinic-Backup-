using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockItemGenArchive")]
public partial class StockItemGenArchive
{
    [StringLength(50)]
    public string? DrgCode { get; set; }

    [StringLength(350)]
    public string DrgName { get; set; } = null!;

    [StringLength(150)]
    public string DrgCatName { get; set; } = null!;

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }

    public double? BulkUnit { get; set; }

    public double? PharmUnit { get; set; }

    public double UnitsInStock { get; set; }

    public double? UnitLevel { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    [StringLength(50)]
    public string? Brand { get; set; }

    public double? Unit2 { get; set; }

    public double? Unit3 { get; set; }
}
