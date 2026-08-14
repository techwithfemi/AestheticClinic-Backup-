using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugMaster
{
    [StringLength(255)]
    [Unicode(false)]
    public string DrgName { get; set; } = null!;

    [StringLength(255)]
    public string? PharmName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? DrgCatName { get; set; }

    [Column("Qty/Unit")]
    [StringLength(43)]
    public string? Qty_Unit { get; set; }

    public double? BulkUnit { get; set; }

    public double? PharmUnit { get; set; }

    public double? UnitsInStock { get; set; }

    public double? UnitLevel { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    [StringLength(50)]
    public string? Brand { get; set; }

    public double? Unit2 { get; set; }

    public double? Unit3 { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(250)]
    public string? PharmCat { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    public long SNo { get; set; }

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

    [StringLength(50)]
    [Unicode(false)]
    public string? DeptID { get; set; }
}
