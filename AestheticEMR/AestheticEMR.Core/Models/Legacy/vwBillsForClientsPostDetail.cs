using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillsForClientsPostDetail
{
    public int? SNO { get; set; }

    public long SNoInv { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InvDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

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

    [StringLength(105)]
    public string? Remarks { get; set; }

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(50)]
    public string? Debit { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Credit { get; set; }

    public double Amount { get; set; }

    [StringLength(550)]
    public string drgName { get; set; } = null!;
}
