using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Table("hExpenseDetailsApprvFinal")]
public partial class hExpenseDetailsApprvFinal
{
    [Key]
    public long ExpID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    [StringLength(500)]
    public string ExpName { get; set; } = null!;

    [StringLength(500)]
    public string ExpCat { get; set; } = null!;

    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SubTotal { get; set; }

    public long? AcctID { get; set; }

    public bool? isApprv { get; set; }

    public bool? AttendedTo { get; set; }

    [StringLength(1000)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateLastPurch { get; set; }

    public double? QtyLastPurch { get; set; }

    public double? PriceLastPurch { get; set; }

    public bool? isPost { get; set; }

    public bool? isPaid { get; set; }

    public bool? isDone { get; set; }

    public bool? suppres { get; set; }

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

    public long? expID_SNo { get; set; }
}
