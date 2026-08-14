using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hPhysio")]
public partial class hPhysio
{
    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(50)]
    public string PNo { get; set; } = null!;

    [StringLength(500)]
    public string? Activity { get; set; }

    [StringLength(500)]
    public string? Coping { get; set; }

    [StringLength(500)]
    public string? Limitation { get; set; }

    [StringLength(250)]
    public string? Tests { get; set; }

    [StringLength(250)]
    public string? Impression { get; set; }

    [StringLength(500)]
    public string? Goals { get; set; }

    [StringLength(500)]
    public string? Means { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReviewDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApptDate { get; set; }

    [StringLength(50)]
    public string? conID { get; set; }

    public int id { get; set; }
}
