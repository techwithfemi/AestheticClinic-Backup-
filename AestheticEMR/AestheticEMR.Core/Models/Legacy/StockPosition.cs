using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("StockPosition")]
public partial class StockPosition
{
    [Key]
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OpenBalDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Cost { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string LocID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OpenBal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? RunningTotalTest { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DRGCode { get; set; }
}
