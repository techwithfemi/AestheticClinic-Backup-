using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("paymentsArchive")]
public partial class paymentsArchive
{
    [Column(TypeName = "datetime")]
    public DateTime ReceiptDate { get; set; }

    [StringLength(50)]
    public string ReceiptNo { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string? clinicID { get; set; }

    [StringLength(100)]
    public string paymentFor { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(550)]
    public string AmountInWord { get; set; } = null!;

    [StringLength(50)]
    public string? Receivedby { get; set; }

    [StringLength(50)]
    public string payType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? rTime { get; set; }

    [StringLength(50)]
    public string? Remarks { get; set; }

    public bool? isPost { get; set; }
}
