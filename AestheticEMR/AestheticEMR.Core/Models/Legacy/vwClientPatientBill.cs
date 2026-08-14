using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwClientPatientBill
{
    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(550)]
    public string drgName { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    public double subTotal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    public int? NumDays { get; set; }

    [StringLength(250)]
    public string diagnosis { get; set; } = null!;

    [StringLength(50)]
    public string? Dosage { get; set; }

    public int? Age { get; set; }

    [StringLength(50)]
    public string? empNo { get; set; }

    [StringLength(50)]
    public string? coyNAme { get; set; }

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    [StringLength(164)]
    public string? Company { get; set; }

    [StringLength(50)]
    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }
}
