using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPaymentDetail
{
    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ReceiptNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RevType { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountToPay { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string AccountNo { get; set; } = null!;

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [StringLength(50)]
    public string? ClientType { get; set; }
}
