using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hAnteNatal")]
public partial class hAnteNatal
{
    [Column(TypeName = "datetime")]
    public DateTime ANdate { get; set; }

    [StringLength(50)]
    public string? gestAge { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? consultID { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? UrineAlb { get; set; }

    [StringLength(50)]
    public string? UrineSug { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BP { get; set; }

    [StringLength(50)]
    public string? wt { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Fundus { get; set; }

    [StringLength(100)]
    public string? presentation { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FH { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Oedema { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? HB { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PCV { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TT { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TCA { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? MO { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? IsDelv { get; set; }
}
