using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

public partial class PaymentForCLient
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? rTime { get; set; }

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string InvNo { get; set; } = null!;

    [StringLength(1000)]
    [Unicode(false)]
    public string? paymentFor { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(550)]
    [Unicode(false)]
    public string AmountInWord { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PayType { get; set; } = null!;

    [StringLength(5000)]
    [Unicode(false)]
    public string? Description { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyCode { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ChequeNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ValueDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BankCode { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ChequeDate { get; set; }

    public bool? isPost { get; set; }

    public bool? isRev { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    public bool? Suppres { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryTime { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ClientName { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? AppName { get; set; }
}
