using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwInvoiceDetailsCoy2Batch
{
    [StringLength(4)]
    public string? BillYear2 { get; set; }

    [StringLength(2)]
    public string? BillMonth2 { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(269)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilled { get; set; }

    [StringLength(50)]
    public string InvNo { get; set; } = null!;

    [StringLength(269)]
    [Unicode(false)]
    public string retainName { get; set; } = null!;

    public bool? Posted { get; set; }

    [StringLength(50)]
    public string? BillYear { get; set; }

    [StringLength(50)]
    public string? BillMonth { get; set; }

    [StringLength(101)]
    public string? Period { get; set; }

    [StringLength(50)]
    public string? AcctID { get; set; }
}
