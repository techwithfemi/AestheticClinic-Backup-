using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockEntryAcct
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Invoicedate { get; set; }

    [StringLength(50)]
    public string POID { get; set; } = null!;

    public long SupplierID { get; set; }

    [StringLength(50)]
    public string SupplierName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AcctDebit { get; set; }

    [StringLength(50)]
    public string? AcctCredit { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Debt { get; set; }

    [StringLength(50)]
    public string AccountName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? InvoiceNo { get; set; }

    public bool isPost { get; set; }

    public int? Yr { get; set; }

    public int? Mth { get; set; }

    public int? PostYr { get; set; }

    public int? PostMth { get; set; }

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

    [StringLength(132)]
    public string? Remarks { get; set; }
}
