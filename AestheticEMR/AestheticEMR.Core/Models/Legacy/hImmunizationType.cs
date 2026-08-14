using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class hImmunizationType
{
    [Key]
    public long SNO { get; set; }

    [StringLength(50)]
    public string ImmType { get; set; } = null!;

    [StringLength(50)]
    public string AgeValue { get; set; } = null!;

    public double Private { get; set; }

    public double? HMO { get; set; }

    public double? NHIS { get; set; }

    public double? NEPA { get; set; }

    public double? MTHLY { get; set; }

    [Column("3MTHLY")]
    public double? _3MTHLY { get; set; }

    [Column("6MTHLY")]
    public double? _6MTHLY { get; set; }

    public double? CBN { get; set; }
}
