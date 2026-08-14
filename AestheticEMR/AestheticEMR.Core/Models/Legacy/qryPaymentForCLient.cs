using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryPaymentForCLient
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string InvNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? Balance { get; set; }

    [StringLength(550)]
    [Unicode(false)]
    public string AmountPaidInWord { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? paymentFor { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PayType { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? ChequeNo { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? BankName { get; set; }

    [StringLength(101)]
    public string? ReceivedBy { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? EmpID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BankCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyCode { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    public bool? isPost { get; set; }

    public bool? isRev { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(328)]
    [Unicode(false)]
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
}
