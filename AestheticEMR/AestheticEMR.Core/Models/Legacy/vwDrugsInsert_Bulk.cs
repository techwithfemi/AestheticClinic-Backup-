using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugsInsert_Bulk
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

    [StringLength(2500)]
    public string? stdPresc { get; set; }

    public double? stdQty { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }

    [StringLength(50)]
    public string? Brand { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DrgCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Dept { get; set; }

    [StringLength(50)]
    public string LocID { get; set; } = null!;
}
