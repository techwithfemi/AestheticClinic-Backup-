using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhBillingReceiptPublic
{
    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? rTime { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    public string paymentFor { get; set; } = null!;

    [StringLength(550)]
    public string AmountInWord { get; set; } = null!;

    [StringLength(50)]
    public string payType { get; set; } = null!;

    [StringLength(50)]
    public string? clinicID { get; set; }

    [StringLength(116)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string coyNAme { get; set; } = null!;
}
