using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugsForGridPriceUpdate
{
    [StringLength(350)]
    public string Drug { get; set; } = null!;

    [StringLength(50)]
    public string Category { get; set; } = null!;

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

    [StringLength(3)]
    public string? Capitated { get; set; }

    public double? UnitPrice { get; set; }
}
