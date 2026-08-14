using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugsZeroPrice
{
    [StringLength(350)]
    public string DrgName { get; set; } = null!;

    [StringLength(50)]
    public string DrgCatName { get; set; } = null!;

    public double UnitsInStock { get; set; }

    [StringLength(2500)]
    public string? stdPresc { get; set; }

    public double? stdQty { get; set; }

    [Column("Qty/Unit")]
    [StringLength(50)]
    public string? Qty_Unit { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? Private { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? NHIS { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? HMO { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string? GENE { get; set; }

    [Column("3MTHLY")]
    public double? _3MTHLY { get; set; }

    [Column("6MTHLY")]
    public double? _6MTHLY { get; set; }

    public double? CBN { get; set; }

    public double? NEPA { get; set; }

    public double? UnitPrice { get; set; }

    public double? ReOrderLevel { get; set; }
}
