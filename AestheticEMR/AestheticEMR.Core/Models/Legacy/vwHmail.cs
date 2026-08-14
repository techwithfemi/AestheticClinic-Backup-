using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwHmail
{
    public long SNo { get; set; }

    [StringLength(101)]
    public string? SentFrom { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? Message { get; set; }

    public bool? isNew { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Status { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? DateSent { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string EmpIDFrom { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string EmpIDTo { get; set; } = null!;

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(101)]
    public string? SentTo { get; set; }
}
