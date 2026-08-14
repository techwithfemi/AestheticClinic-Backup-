using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("PaymentAdjust")]
public partial class PaymentAdjust
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ReceiptNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountOriginal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountNew { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AdjustType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime AdjustTime { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? AdjustBy { get; set; }
}
