using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhBillingReceiptRpt
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Time { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(19, 0)")]
    public decimal? Balance { get; set; }

    [StringLength(50)]
    public string? Company { get; set; }

    [StringLength(100)]
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
    public string BillNo { get; set; } = null!;
}
