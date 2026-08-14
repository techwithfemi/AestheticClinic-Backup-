using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillsForClientsPost
{
    public long SNO { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountInvoiced { get; set; }

    [StringLength(50)]
    public string InvoiceNo { get; set; } = null!;

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(6)]
    public string? BatchVal { get; set; }

    [StringLength(50)]
    public string? Mth { get; set; }

    [StringLength(50)]
    public string? Yr { get; set; }

    public bool? isPost { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string Posted { get; set; } = null!;

    [StringLength(50)]
    public string? AcctID { get; set; }

    [StringLength(105)]
    public string? Remarks { get; set; }
}
