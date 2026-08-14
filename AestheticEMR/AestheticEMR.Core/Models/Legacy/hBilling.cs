using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hBilling")]
public partial class hBilling
{
    [Column(TypeName = "smalldatetime")]
    public DateTime bDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime consultDate { get; set; }

    [StringLength(100)]
    public string? paymentFor { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "ntext")]
    public string? AmountInWord { get; set; }

    [StringLength(50)]
    public string? Receivedby { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? balanceDate { get; set; }

    [StringLength(50)]
    public string? payType { get; set; }
}
