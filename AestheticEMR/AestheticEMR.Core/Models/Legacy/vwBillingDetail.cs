using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingDetail
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? oldpNo { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmtBF { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(20, 0)")]
    public decimal? Balance { get; set; }

    [StringLength(950)]
    public string? AmountBilledInWord { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    public bool? isPaid { get; set; }
}
