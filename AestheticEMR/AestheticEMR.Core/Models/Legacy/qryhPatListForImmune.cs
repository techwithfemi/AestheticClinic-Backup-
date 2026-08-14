using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPatListForImmune
{
    [StringLength(100)]
    [Unicode(false)]
    public string PNo { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string pSurname { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? pFirstname { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    public bool? attendedTo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [StringLength(100)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    public int? Age { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string company { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? coyNAme { get; set; }

    public int recID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? htime { get; set; }

    public bool? suppres { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string retainName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? OldPNo { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(557)]
    [Unicode(false)]
    public string? FullName { get; set; }

    public bool? AttendedToByImmume { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Expr1 { get; set; } = null!;
}
