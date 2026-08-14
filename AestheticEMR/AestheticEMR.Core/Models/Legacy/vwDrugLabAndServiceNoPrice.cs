using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugLabAndServiceNoPrice
{
    [Column("Drug/Service")]
    [StringLength(350)]
    public string Drug_Service { get; set; } = null!;

    [StringLength(350)]
    public string Category { get; set; } = null!;

    public double? PRIVATE { get; set; }

    public double? NHIS { get; set; }

    public double? HMO { get; set; }

    public double? HospCoy { get; set; }

    [Column("3MTHLY")]
    public double? _3MTHLY { get; set; }

    [Column("6MTHLY")]
    public double? _6MTHLY { get; set; }

    public double? CBN { get; set; }

    public double? NEPA { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;
}
