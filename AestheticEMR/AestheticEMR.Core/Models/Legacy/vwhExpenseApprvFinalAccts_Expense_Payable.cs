using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseApprvFinalAccts_Expense_Payable
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    public long ItemCode { get; set; }

    [StringLength(255)]
    public string ItemName { get; set; } = null!;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SubTotal { get; set; }

    public bool Suppres { get; set; }

    [StringLength(1333)]
    public string? Remarks { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string CatCode { get; set; } = null!;

    [StringLength(225)]
    [Unicode(false)]
    public string CatType { get; set; } = null!;

    [StringLength(225)]
    [Unicode(false)]
    public string CatName { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string AcctDebit { get; set; } = null!;

    [StringLength(50)]
    public string? AcctCredit { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpDate { get; set; }

    public bool? isPost { get; set; }

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
}
