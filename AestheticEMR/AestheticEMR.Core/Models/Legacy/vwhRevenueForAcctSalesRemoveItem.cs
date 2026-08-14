using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhRevenueForAcctSalesRemoveItem
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [StringLength(50)]
    public string CoyID { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmtPaid { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? AmtDiff { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(50)]
    public string? AcctDebit { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AcctCredit { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string? Active { get; set; }

    public bool isPost { get; set; }

    public bool? suppres { get; set; }

    public bool? Reversed { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
