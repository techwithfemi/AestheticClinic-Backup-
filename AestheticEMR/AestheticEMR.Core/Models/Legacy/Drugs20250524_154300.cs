using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("Drugs20250524_154300")]
public partial class Drugs20250524_154300
{
    [StringLength(255)]
    [Unicode(false)]
    public string DrgName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LocID { get; set; } = null!;

    [StringLength(350)]
    [Unicode(false)]
    public string? PharmName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? DrgCatName { get; set; }

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? BulkUnit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PharmUnit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitsInStock { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ReOrderLevel { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Brand { get; set; }

    public int? Unit2 { get; set; }

    public int? Unit3 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Capitated { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? PharmCat { get; set; }

    [StringLength(2500)]
    public string? stdPresc { get; set; }

    public int? stdQty { get; set; }

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

    [StringLength(50)]
    [Unicode(false)]
    public string? DrgCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastQtyInStock { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastDatePurch { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastQtyPurch { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastUnitPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LastPOID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PrevLastQtyInStock { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyPurch { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? POID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DatePurch { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LastQtyIssued { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastDateIssued { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyIssued { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateIssued { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PCentMargin { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SellingPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PriceMargin { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? LeadLevel { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? BulkUnit2 { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? QtyUsed { get; set; }

    public int? UnitLevel { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? BulkCost { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? Amount { get; set; }
}
