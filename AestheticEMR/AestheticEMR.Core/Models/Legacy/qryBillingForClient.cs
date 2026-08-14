using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingForClient
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(150)]
    public string CLIENTName { get; set; } = null!;

    [StringLength(50)]
    public string? oldpNo { get; set; }

    [StringLength(101)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? consultDate { get; set; }

    [StringLength(50)]
    public string? clientID { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountBilled { get; set; }

    [StringLength(250)]
    public string? AmountBilledInWord { get; set; }

    [Column(TypeName = "numeric(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    [StringLength(250)]
    public string diagnosis { get; set; } = null!;

    public bool? isPaid { get; set; }
}
