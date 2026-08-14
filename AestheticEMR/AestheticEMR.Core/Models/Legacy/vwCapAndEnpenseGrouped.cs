using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwCapAndEnpenseGrouped
{
    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string CoyName { get; set; } = null!;

    [StringLength(2)]
    public string Mth { get; set; } = null!;

    [StringLength(4)]
    public string? Yr { get; set; }

    [Column("PHIS-INCOME")]
    public double? PHIS_INCOME { get; set; }

    [Column("NHIS-INCOME")]
    public double? NHIS_INCOME { get; set; }

    [Column("NHIS-EXPENSE")]
    public double? NHIS_EXPENSE { get; set; }

    [Column("NHIS-FFS-INCOME")]
    public double? NHIS_FFS_INCOME { get; set; }

    [Column("PHIS-EXPENSE")]
    public double? PHIS_EXPENSE { get; set; }

    [Column("PHIS-FFS-INCOME")]
    public double? PHIS_FFS_INCOME { get; set; }
}
