using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryPayment
{
    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(1000)]
    public string paymentFor { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string RetainName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountBilled { get; set; }

    public int DebtBF { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
