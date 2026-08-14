using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPaymentDetailsSumm2
{
    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountPaidDetail { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceiptDateDetail { get; set; }

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? Diff { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(1000)]
    public string paymentFor { get; set; } = null!;
}
