using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpensePaySumm
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string? VouchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountToPay { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [StringLength(71)]
    public string? RefNo { get; set; }

    [StringLength(3550)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(101)]
    public string? PaidBy { get; set; }

    [StringLength(150)]
    public string? Recipient { get; set; }

    public bool? isPost { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Posted { get; set; } = null!;

    [StringLength(50)]
    public string? Apprvdby { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountApprved { get; set; }

    [StringLength(50)]
    public string? Apprvby { get; set; }
}
