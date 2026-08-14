using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPaymentTypesForAcct
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReceiptDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? AccountNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string PayType { get; set; } = null!;

    public bool? isPost { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(1000)]
    public string paymentFor { get; set; } = null!;

    [StringLength(1122)]
    public string Remarks { get; set; } = null!;

    public bool? suppres { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Reversal { get; set; }
}
