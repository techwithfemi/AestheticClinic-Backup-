using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhPatListForDialysi
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

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? OldPNo { get; set; }
}
