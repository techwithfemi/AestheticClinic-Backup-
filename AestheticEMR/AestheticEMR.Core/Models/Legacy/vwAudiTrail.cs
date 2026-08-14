using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAudiTrail
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Time { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string UserAction { get; set; } = null!;

    [Unicode(false)]
    public string? OriginalAction { get; set; }

    [Unicode(false)]
    public string? Remarks { get; set; }

    [Unicode(false)]
    public string? Src { get; set; }

    public string? Employee { get; set; }

    [StringLength(550)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string TranCode { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? Module { get; set; }
}
