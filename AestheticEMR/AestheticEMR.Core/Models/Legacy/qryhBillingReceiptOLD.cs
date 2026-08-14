using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhBillingReceiptOLD
{
    [Column(TypeName = "smalldatetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(100)]
    public string paymentFor { get; set; } = null!;

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "ntext")]
    public string? AmountInWord { get; set; }

    [StringLength(50)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string payType { get; set; } = null!;
}
