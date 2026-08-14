using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("LabServicesOLD")]
public partial class LabServicesOLD
{
    [StringLength(10)]
    [Unicode(false)]
    public string? LabID { get; set; }

    [Key]
    [StringLength(550)]
    [Unicode(false)]
    public string DrgName { get; set; } = null!;

    [StringLength(550)]
    public string? DrgCatName { get; set; }

    [StringLength(50)]
    public string? qtyPerUnit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Private { get; set; }

    public double? NHIS { get; set; }

    public double? HMO { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? ADC { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    [Column("3MTHLY")]
    public double? _3MTHLY { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    [Column("6MTHLY")]
    public double? _6MTHLY { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? CBN { get; set; }

    /// <summary>
    /// 0
    /// </summary>
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

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? ResultTemplate { get; set; }
}
