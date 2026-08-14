using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwClientBill
{
    [Column(TypeName = "datetime")]
    public DateTime bDate { get; set; }

    [StringLength(301)]
    public string Fullname { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [StringLength(150)]
    public string? clientID { get; set; }

    [StringLength(50)]
    public string? BillingMonth { get; set; }

    public int? BillingYear { get; set; }

    [StringLength(150)]
    public string ClientName { get; set; } = null!;
}
