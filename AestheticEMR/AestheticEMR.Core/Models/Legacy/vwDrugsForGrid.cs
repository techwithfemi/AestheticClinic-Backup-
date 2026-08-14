using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugsForGrid
{
    [StringLength(255)]
    [Unicode(false)]
    public string Drug { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Category { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitsInStock { get; set; }

    [StringLength(2500)]
    public string? stdPresc { get; set; }

    public double? stdQty { get; set; }

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

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ReOrderLevel { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? BulkUnit { get; set; }

    public int? UnitLevel { get; set; }

    public int? Unit2 { get; set; }

    public int? Unit3 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Capitated { get; set; }

    [StringLength(250)]
    public string? Remarks { get; set; }

    [StringLength(255)]
    public string? PharmName { get; set; }

    [StringLength(150)]
    public string? Location { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DrgCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LocID { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiredDate { get; set; }

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DeptID2 { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PCentMargin { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SellingPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Brand { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? StockDept { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? BarCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DeptID { get; set; }
}
