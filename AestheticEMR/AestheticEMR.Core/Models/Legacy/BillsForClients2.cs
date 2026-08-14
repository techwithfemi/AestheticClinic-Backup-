using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("BillsForClients2")]
public partial class BillsForClients2
{
    public long SNO { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime InvDate { get; set; }

    [StringLength(50)]
    public string InvNo { get; set; } = null!;

    [StringLength(50)]
    public string? CoyCode { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmountInvoiced { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountPaid { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? AmtBF { get; set; }

    [StringLength(950)]
    public string? AmountBilledInWord { get; set; }

    [StringLength(50)]
    public string? BillMonth { get; set; }

    [StringLength(50)]
    public string? BillYear { get; set; }

    public bool? isPaid { get; set; }

    [StringLength(50)]
    public string? BatchNo { get; set; }

    public bool? isOLd { get; set; }
}
