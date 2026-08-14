using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceDetailsCoy2BatchForPost
{
    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [StringLength(269)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string InvoiceNo { get; set; } = null!;

    [StringLength(50)]
    public string? Debit { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountInvoiced { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Posted { get; set; } = null!;

    public bool? isPost { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(4)]
    [Unicode(false)]
    public string Credit { get; set; } = null!;

    [StringLength(50)]
    public string? BillMonth { get; set; }

    [StringLength(50)]
    public string? BillYear { get; set; }
}
