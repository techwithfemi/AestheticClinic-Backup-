using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhAntenatal
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FundusHeight { get; set; }

    [StringLength(100)]
    public string? presentation { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Relation { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FoetalHeart { get; set; }

    [StringLength(50)]
    public string? UrineAlbumen { get; set; }

    [StringLength(50)]
    public string? UrineSugar { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BP { get; set; }

    [StringLength(50)]
    public string? wt { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PCV { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Oedema { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? gestAge { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? consultID { get; set; }
}
