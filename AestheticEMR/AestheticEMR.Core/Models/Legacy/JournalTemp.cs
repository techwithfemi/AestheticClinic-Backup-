using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("JournalTemp")]
public partial class JournalTemp
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DtDate { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 18)")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Description { get; set; }

    public long TableSNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TableName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string AcctGp { get; set; } = null!;

    public bool AttendedTo { get; set; }

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

    public bool? suppres { get; set; }
}
