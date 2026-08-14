using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwCapAndEnpense
{
    [StringLength(50)]
    public string? CoyName { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    public int? Yr { get; set; }

    public double? Amount { get; set; }

    [StringLength(11)]
    [Unicode(false)]
    public string HmoClass { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;
}
