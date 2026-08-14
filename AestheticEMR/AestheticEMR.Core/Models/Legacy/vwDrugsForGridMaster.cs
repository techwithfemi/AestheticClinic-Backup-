using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugsForGridMaster
{
    [StringLength(255)]
    public string Drug { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Category { get; set; }

    public double? UnitsInStock { get; set; }

    [StringLength(2500)]
    public string? stdPresc { get; set; }

    public double? stdQty { get; set; }

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }

    public double? PRIVATE { get; set; }

    public double? NHIS { get; set; }

    public double? HMO { get; set; }

    public double? GENE { get; set; }

    [Column("3MTHLY")]
    public double? _3MTHLY { get; set; }

    [Column("6MTHLY")]
    public double? _6MTHLY { get; set; }

    public double? CBN { get; set; }

    public double? NEPA { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    public double? BulkUnit { get; set; }

    public double? UnitLevel { get; set; }

    public double? Unit2 { get; set; }

    public double? Unit3 { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }

    [StringLength(255)]
    public string? PharmName { get; set; }

    [StringLength(9)]
    [Unicode(false)]
    public string? DrgCode { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    public double? PharmUnit { get; set; }

    [StringLength(50)]
    public string? Brand { get; set; }

    public long SNo { get; set; }
}
