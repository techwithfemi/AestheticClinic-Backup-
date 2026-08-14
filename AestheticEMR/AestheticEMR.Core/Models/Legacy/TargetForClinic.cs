using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("TargetForClinic")]
public partial class TargetForClinic
{
    public long SNo { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string Mth { get; set; } = null!;

    [StringLength(4)]
    [Unicode(false)]
    public string Yr { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string TargetID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Actual { get; set; }

    public bool isMet { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
