using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hAncObsteHist")]
public partial class hAncObsteHist
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string ANCRegNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string pNo { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? dtDelv { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? gestAge { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? delvMode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? wtAtBirth { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? sex { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? compAtBirth { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Alive { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? hosp { get; set; }
}
