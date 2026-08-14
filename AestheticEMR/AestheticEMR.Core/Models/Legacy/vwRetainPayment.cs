using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwRetainPayment
{
    [StringLength(150)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountDue { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(550)]
    public string AmountInWord { get; set; } = null!;

    [StringLength(50)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string payType { get; set; } = null!;

    [StringLength(50)]
    public string? ChequeNo { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;
}
