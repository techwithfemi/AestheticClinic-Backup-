using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseForRpt2
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(500)]
    public string? ExpenseBy { get; set; }

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    [StringLength(100)]
    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    public double? SubTotal { get; set; }

    [StringLength(500)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? QtyFinalApprv { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PriceFinalApprv { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountFinalApprv { get; set; }

    [StringLength(101)]
    public string? FinalApprvalby { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(101)]
    public string? PaidBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DatePaid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TimePaid { get; set; }

    [StringLength(50)]
    public string ExpCat { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? AcctID { get; set; }

    [StringLength(50)]
    public string? PersNo { get; set; }

    [StringLength(50)]
    public string Dept { get; set; } = null!;
}
