using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingForClaimsByPatInvoice
{
    public int Sno { get; set; }

    public double? SubTotal { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(550)]
    public string? BilltRemarks { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? dischDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AdmDate { get; set; }

    [StringLength(50)]
    public string ClinicType { get; set; } = null!;

    [StringLength(57)]
    public string? Age { get; set; }

    [StringLength(100)]
    public string? BillToNo { get; set; }

    [StringLength(163)]
    public string Company { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }
}
