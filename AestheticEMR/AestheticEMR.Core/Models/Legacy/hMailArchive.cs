using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hMailArchive")]
public partial class hMailArchive
{
    public long? SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateSent { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Title { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? EmpIDFrom { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? EmpIDTo { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Message { get; set; }

    public bool? isNew { get; set; }
}
