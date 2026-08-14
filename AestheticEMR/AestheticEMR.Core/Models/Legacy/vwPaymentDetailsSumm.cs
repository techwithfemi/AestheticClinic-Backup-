using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPaymentDetailsSumm
{
    [StringLength(50)]
    [Unicode(false)]
    public string ReceiptNo { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountPaid { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;
}
