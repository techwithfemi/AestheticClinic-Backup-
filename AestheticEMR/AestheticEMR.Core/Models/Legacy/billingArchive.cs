using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("billingArchive")]
public partial class billingArchive
{
    public long ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? consultDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string? clientID { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal profFee { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmtBF { get; set; }

    [StringLength(950)]
    public string? AmountBilledInWord { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [StringLength(50)]
    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    [StringLength(250)]
    public string diagnosis { get; set; } = null!;

    public bool? isPaid { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? billType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? BillTo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? CoyName { get; set; }
}
