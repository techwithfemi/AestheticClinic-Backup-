using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryHDrugsX
{
    [StringLength(250)]
    public string Drug { get; set; } = null!;

    [StringLength(50)]
    public string Category { get; set; } = null!;

    [Column("Qty/Unit")]
    [StringLength(50)]
    public string? Qty_Unit { get; set; }

    public double Cost { get; set; }

    public double? NHIS { get; set; }

    public double? price { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string remarks { get; set; } = null!;

    [StringLength(50)]
    public string? deptBillCenter { get; set; }

    [StringLength(2500)]
    public string? StdPresc { get; set; }

    public double? StdQty { get; set; }

    public double? UnitsInStock { get; set; }
}
