using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingForClaimsByPatGrouped
{
    [StringLength(1001)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? SubTotal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime consultDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? oldpNo { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? diagnosis { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? AmountBilled { get; set; }

    [StringLength(950)]
    public string? AmountBilledInWord { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyName { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? drgcatGroup { get; set; }

    [StringLength(57)]
    public string? Age { get; set; }

    [StringLength(100)]
    public string? empNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [StringLength(8000)]
    [Unicode(false)]
    public string? ApprvCode { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(500)]
    public string? email { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? policyType { get; set; }

    [StringLength(268)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Referal { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillTo { get; set; }

    [StringLength(100)]
    public string? BillToNo { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(50)]
    public string? revType { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal Payment { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilledPay { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Discount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Debt { get; set; }
}
