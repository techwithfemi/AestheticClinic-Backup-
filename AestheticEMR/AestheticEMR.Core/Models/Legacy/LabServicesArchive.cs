using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("LabServicesArchive")]
public partial class LabServicesArchive
{
    [StringLength(10)]
    [Unicode(false)]
    public string? LabID { get; set; }

    [StringLength(550)]
    [Unicode(false)]
    public string DrgName { get; set; } = null!;

    [StringLength(550)]
    public string? DrgCatName { get; set; }

    [StringLength(50)]
    public string? qtyPerUnit { get; set; }

    [Column(TypeName = "money")]
    public decimal? Private { get; set; }

    public double? NHIS { get; set; }

    public double? HMO { get; set; }

    public double? ADC { get; set; }

    [Column("3MTHLY")]
    public double? _3MTHLY { get; set; }

    [Column("6MTHLY")]
    public double? _6MTHLY { get; set; }

    public double? CBN { get; set; }

    public double? NEPA { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? RangeVal { get; set; }

    [StringLength(50)]
    public string? TestUnit { get; set; }

    [StringLength(150)]
    public string? Class { get; set; }

    [StringLength(50)]
    public string? Range { get; set; }

    [StringLength(50)]
    public string? Sample { get; set; }
}
