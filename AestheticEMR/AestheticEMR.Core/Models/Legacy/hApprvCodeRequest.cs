using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hApprvCodeRequest")]
public partial class hApprvCodeRequest
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ApprvDate { get; set; }

    [StringLength(50)]
    public string? ConsultID { get; set; }

    [StringLength(550)]
    public string? Remarks { get; set; }

    public bool? isSent { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? EnrolleeNo { get; set; }
}
