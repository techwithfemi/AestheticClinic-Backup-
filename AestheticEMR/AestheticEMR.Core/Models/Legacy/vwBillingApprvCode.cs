using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingApprvCode
{
    [Column(TypeName = "datetime")]
    public DateTime AttdDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime BillDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string? RetainCode { get; set; }

    [StringLength(150)]
    public string CoyName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    public string? ApprvCode { get; set; }

    public int? MonthCode { get; set; }

    public int? YearCode { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }

    public bool? isProcess { get; set; }

    [StringLength(50)]
    public string? InvNo { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(100)]
    public string? Remarks { get; set; }

    public long ID { get; set; }
}
