using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseApprvFinal
{
    public long ExpID { get; set; }

    public long SNo { get; set; }

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    public long ItemCode { get; set; }

    [StringLength(500)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SubTotal { get; set; }

    [StringLength(500)]
    public string? ReceivedBy { get; set; }

    [StringLength(101)]
    public string? ApprvBy { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? AcctID { get; set; }

    [StringLength(71)]
    public string? RefNo { get; set; }

    public bool? isPost { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? isApprv { get; set; }

    public bool? isPaid { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountApprved { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string CatCode { get; set; } = null!;

    [StringLength(225)]
    [Unicode(false)]
    public string CatName { get; set; } = null!;

    [StringLength(225)]
    [Unicode(false)]
    public string CatType { get; set; } = null!;

    public bool? suppres { get; set; }

    [StringLength(1578)]
    public string Remarks { get; set; } = null!;

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
