using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hDebtReview")]
public partial class hDebtReview
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DtDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BillNo { get; set; } = null!;

    public double Debt { get; set; }

    public double AdjustTo { get; set; }

    public bool? attendedto { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }
}
