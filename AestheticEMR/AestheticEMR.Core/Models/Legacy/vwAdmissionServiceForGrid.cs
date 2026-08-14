using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAdmissionServiceForGrid
{
    [StringLength(250)]
    public string Drug { get; set; } = null!;

    [StringLength(50)]
    public string Category { get; set; } = null!;

    [Column("Qty/Unit")]
    [StringLength(50)]
    public string? Qty_Unit { get; set; }

    public double? PRIVATE { get; set; }

    public double? NHIS { get; set; }

    public double? HMO { get; set; }

    public double? MTHLY { get; set; }

    [Column("3MTHLY")]
    public double? _3MTHLY { get; set; }

    [Column("6MTHLY")]
    public double? _6MTHLY { get; set; }

    public double? CBN { get; set; }

    public double? NEPA { get; set; }
}
