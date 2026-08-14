using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("LabServicesExcelEmel")]
public partial class LabServicesExcelEmel
{
    [StringLength(10)]
    [Unicode(false)]
    public string? LabID { get; set; }

    [StringLength(255)]
    public string? DrgName { get; set; }

    [StringLength(255)]
    public string? DrgCatName { get; set; }

    [StringLength(50)]
    public string? qtyPerUnit { get; set; }

    public double? Private { get; set; }

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
}
