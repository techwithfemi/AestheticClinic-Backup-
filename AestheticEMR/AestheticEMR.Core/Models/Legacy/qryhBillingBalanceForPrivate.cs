using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhBillingBalanceForPrivate
{
    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(100)]
    public string pNo { get; set; } = null!;

    [StringLength(1100)]
    [Unicode(false)]
    public string? homeAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountPaid { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? Balance { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    public bool? isPaid { get; set; }

    [StringLength(251)]
    public string Fullname { get; set; } = null!;

    [StringLength(950)]
    public string? AmountBilledInWord { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? clientCatID { get; set; }

    [StringLength(50)]
    public string? RetainCode { get; set; }
}
