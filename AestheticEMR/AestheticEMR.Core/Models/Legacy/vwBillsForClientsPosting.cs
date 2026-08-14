using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillsForClientsPosting
{
    public long SNO { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(50)]
    public string? AcctID { get; set; }

    public double? Debt { get; set; }

    [StringLength(50)]
    public string? DebtType { get; set; }

    [StringLength(50)]
    public string InvNo { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountInvoiced { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmtBF { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    public bool? isPost { get; set; }

    public bool? isOLd { get; set; }

    [StringLength(169)]
    public string? Remarks { get; set; }

    public bool? AttendedToByClient { get; set; }

    public DateOnly? InvoiceDate { get; set; }

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? Balance { get; set; }
}
