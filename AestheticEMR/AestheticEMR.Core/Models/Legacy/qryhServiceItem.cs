using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhServiceItem
{
    [StringLength(100)]
    public string? Clinic { get; set; }

    [StringLength(100)]
    public string SERVICE { get; set; } = null!;

    [StringLength(100)]
    public string CATEGORY { get; set; } = null!;

    public double? PRIVATE { get; set; }

    public double? NHIS { get; set; }

    public double? HMO { get; set; }

    public double? MTHLY { get; set; }

    [Column("3MTHLY")]
    public double? _3MTHLY { get; set; }

    [Column("6MTHLY")]
    public double? _6MTHLY { get; set; }

    public double? NEPA { get; set; }

    public double? CBN { get; set; }

    [StringLength(100)]
    public string? Type { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }
}
