using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hExpensePay")]
public partial class hExpensePay
{
    public long SNo { get; set; }

    [StringLength(50)]
    public string? VouchNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PayDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PayTime { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [StringLength(50)]
    public string? PaidBy { get; set; }

    [StringLength(3550)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(150)]
    public string? Recipient { get; set; }

    [StringLength(50)]
    public string? payType { get; set; }

    [StringLength(3550)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? ChequeNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ValueDate { get; set; }

    [StringLength(50)]
    public string? BankCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ChequeDate { get; set; }

    public bool? isPost { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AcctNoDebit { get; set; }
}
