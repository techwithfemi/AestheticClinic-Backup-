using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingBalanceForPrivate
{
    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? consultDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(150)]
    public string ClientName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? profFee { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmtBF { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(1250)]
    public string diagnosis { get; set; } = null!;

    [Column(TypeName = "decimal(19, 0)")]
    public decimal? CurrentDebt { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    public string? clientID { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? billType { get; set; }

    public bool? isPaid { get; set; }

    [StringLength(950)]
    public string? AmountBilledInWord { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    public double Debt { get; set; }
}
