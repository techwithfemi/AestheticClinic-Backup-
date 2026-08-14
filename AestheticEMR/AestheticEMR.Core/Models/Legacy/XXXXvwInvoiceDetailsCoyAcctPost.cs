using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class XXXXvwInvoiceDetailsCoyAcctPost
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }

    [StringLength(163)]
    public string Company { get; set; } = null!;

    public double? AmountBilled { get; set; }

    [StringLength(50)]
    public string? InvNo { get; set; }

    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    public double AmountInvoiced { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    public bool? isPost { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }

    [StringLength(50)]
    public string InvoiceNo { get; set; } = null!;
}
