using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBirthday
{
    [StringLength(100)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DOB { get; set; }

    [StringLength(500)]
    public string? email { get; set; }

    public int? Age { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Company { get; set; }
}
