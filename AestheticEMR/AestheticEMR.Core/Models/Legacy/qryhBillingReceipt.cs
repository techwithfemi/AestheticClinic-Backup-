using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhBillingReceipt
{
    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? rTime { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(1000)]
    public string paymentFor { get; set; } = null!;

    [StringLength(550)]
    public string AmountInWord { get; set; } = null!;

    [StringLength(50)]
    public string payType { get; set; } = null!;

    [StringLength(50)]
    public string? clinicID { get; set; }

    [StringLength(101)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    public bool? isPost { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Remarks { get; set; }

    public bool? suppres { get; set; }
}
