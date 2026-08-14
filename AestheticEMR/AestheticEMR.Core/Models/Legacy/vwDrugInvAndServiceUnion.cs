using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugInvAndServiceUnion
{
    [StringLength(255)]
    public string? Service { get; set; }

    public double? Price { get; set; }

    [StringLength(255)]
    public string? Company { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? RevType { get; set; }

    [StringLength(50)]
    public string? Capitated { get; set; }
}
