using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingProcessDetailsGroupedVerifyDiff
{
    [Column(TypeName = "datetime")]
    public DateTime AttdDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BillDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string RetainCode { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? ApprvCode { get; set; }

    public int? YearCode { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    public bool? isProcess { get; set; }

    [StringLength(50)]
    public string? InvNo { get; set; }

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    public long ID { get; set; }

    [StringLength(2)]
    public string? MonthCode { get; set; }

    [StringLength(7)]
    public string? BatchNo { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }
}
