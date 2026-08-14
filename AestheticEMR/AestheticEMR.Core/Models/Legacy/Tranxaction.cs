using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("Tranxaction")]
public partial class Tranxaction
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PNo { get; set; }

    [StringLength(50)]
    public string BillNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal RunningTotal { get; set; }

    [StringLength(200)]
    public string? Remarks { get; set; }

    public bool? isRev { get; set; }

    public int seed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TranStatus { get; set; }
}
