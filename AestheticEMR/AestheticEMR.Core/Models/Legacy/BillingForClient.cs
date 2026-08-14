using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class BillingForClient
{
    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? consultDate { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(50)]
    public string pNo { get; set; } = null!;

    [StringLength(50)]
    public string clientID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [StringLength(250)]
    public string? AmountBilledInWord { get; set; }

    [StringLength(50)]
    public string BillingMonth { get; set; } = null!;

    public int BillingYear { get; set; }

    [StringLength(250)]
    public string diagnosis { get; set; } = null!;

    public bool isPaid { get; set; }
}
