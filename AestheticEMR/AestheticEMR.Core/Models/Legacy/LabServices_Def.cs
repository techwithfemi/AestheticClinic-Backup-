using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("LabServices_Def")]
public partial class LabServices_Def
{
    [StringLength(100)]
    public string DrgName { get; set; } = null!;

    [StringLength(350)]
    public string DrgCatName { get; set; } = null!;

    [StringLength(50)]
    public string? qtyPerUnit { get; set; }

    public double? NHIS { get; set; }

    public double? HMO { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? ADC { get; set; }

    /// <summary>
    /// 0
    /// </summary>
    public double? PRIVATE { get; set; }

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
}
