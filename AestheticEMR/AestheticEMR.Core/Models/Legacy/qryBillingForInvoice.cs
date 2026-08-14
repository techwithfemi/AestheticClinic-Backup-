using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingForInvoice
{
    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime consultDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(250)]
    public string diagnosis { get; set; } = null!;

    [StringLength(550)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [StringLength(950)]
    public string? AmountBilledInWord { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? pCatID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? drgCatGroup { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CatRemarks { get; set; }
}
