using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingForCapByPat
{
    public int Sno { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string CatRemarks { get; set; } = null!;

    [StringLength(17)]
    [Unicode(false)]
    public string BilltRemarks { get; set; } = null!;

    [StringLength(550)]
    public string Service { get; set; } = null!;

    [StringLength(550)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ConsultDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? oldpNo { get; set; }

    [StringLength(1050)]
    public string? diagnosis { get; set; }

    public int AmountBilled { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string AmountBilledInWord { get; set; } = null!;

    public int AmountPaid { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string drgCatGroup { get; set; } = null!;

    [StringLength(57)]
    public string? Age { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string ApprvCode { get; set; } = null!;

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(500)]
    public string? email { get; set; }

    [StringLength(50)]
    public string? policyType { get; set; }

    [StringLength(160)]
    public string? Company { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? referal { get; set; }
}
