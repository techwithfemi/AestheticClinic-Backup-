using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwhPaymentsPublic
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(100)]
    public string paymentFor { get; set; } = null!;

    [StringLength(61)]
    public string Fullname { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;

    [StringLength(50)]
    public string? Remarks { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string billtype { get; set; } = null!;
}
