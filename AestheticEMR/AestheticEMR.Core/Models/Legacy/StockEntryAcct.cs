using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockEntryAcct")]
public partial class StockEntryAcct
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(50)]
    public string POID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public bool isPost { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string SuppAcctID { get; set; } = null!;

    public long SuppID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? InvoiceNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? InvAcctNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PostDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate2 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }

    public bool? suppres { get; set; }

    public bool? AttendedToByVouch { get; set; }
}
