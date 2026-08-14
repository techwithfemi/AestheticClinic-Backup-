using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockBalance")]
public partial class StockBalance
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string ItemID { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LocID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OpenBal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? StockIn { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? StockOut { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? StockUsed { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CloseBal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }
}
