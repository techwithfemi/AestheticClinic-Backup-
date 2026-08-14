using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhExpenseOLD2
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ExpDate { get; set; }

    [StringLength(50)]
    public string VouchNo { get; set; } = null!;

    [StringLength(50)]
    public string Paidby { get; set; } = null!;

    [StringLength(101)]
    public string? Apprvdby { get; set; }

    [StringLength(255)]
    public string ExpName { get; set; } = null!;

    [StringLength(100)]
    public string? Description { get; set; }

    public double Qty { get; set; }

    public double Price { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal Amount { get; set; }

    [StringLength(550)]
    public string? AmountInWord { get; set; }

    [StringLength(101)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string payType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ExpTime { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [StringLength(50)]
    public string? ChequeNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ValueDate { get; set; }

    [StringLength(50)]
    public string? BankCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ChequeDate { get; set; }

    [StringLength(101)]
    public string? EmpName { get; set; }
}
