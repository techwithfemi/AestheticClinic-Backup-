using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingForCapByPatPrivate
{
    public int Sno { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? CatRemarks { get; set; }

    [StringLength(17)]
    [Unicode(false)]
    public string BilltRemarks { get; set; } = null!;

    [StringLength(550)]
    public string Service { get; set; } = null!;

    [StringLength(550)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double? Qty { get; set; }

    public double? SubTotal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime consultDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? oldpNo { get; set; }

    [StringLength(1250)]
    public string diagnosis { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [StringLength(950)]
    public string? AmountBilledInWord { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
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

    [StringLength(50)]
    public string? empNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [StringLength(50)]
    public string? ApprvCode { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(500)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(163)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(30)]
    public string? Referal { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillTo { get; set; }

    [StringLength(100)]
    public string? BillToNo { get; set; }
}
