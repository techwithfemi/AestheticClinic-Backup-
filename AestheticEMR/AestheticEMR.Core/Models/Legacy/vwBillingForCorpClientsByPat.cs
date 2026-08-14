using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingForCorpClientsByPat
{
    public int Sno { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [StringLength(17)]
    [Unicode(false)]
    public string BilltRemarks { get; set; } = null!;

    public double? SubTotal { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? oldpNo { get; set; }

    [StringLength(1250)]
    public string diagnosis { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyName { get; set; }

    [StringLength(57)]
    public string? Age { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [StringLength(50)]
    public string? Sex { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? pPhoneNo { get; set; }

    [StringLength(500)]
    public string? email { get; set; }

    [StringLength(163)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? BillTo { get; set; }

    [StringLength(100)]
    public string? BillToNo { get; set; }

    [StringLength(50)]
    public string clinicType { get; set; } = null!;

    [StringLength(50)]
    public string? pCatID { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }
}
