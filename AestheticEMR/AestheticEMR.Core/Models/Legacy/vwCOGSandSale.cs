using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwCOGSandSale
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(50)]
    public string ConsultID { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? CostAmount { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? SalesAmount { get; set; }

    public bool? isPost { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    public bool? suppres { get; set; }

    [StringLength(1334)]
    public string Remarks { get; set; } = null!;

    [Column(TypeName = "decimal(38, 4)")]
    public decimal? Margin { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TranID { get; set; }

    public long? ReversedPair { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Remarks2 { get; set; }

    public bool? Reversed { get; set; }
}
