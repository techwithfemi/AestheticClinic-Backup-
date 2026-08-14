using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingProcessDetail
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(406)]
    public string Fullname { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Coyname { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Service { get; set; } = null!;

    public double Price { get; set; }

    public double Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? subTotal { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? billType { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    public bool? isProcess { get; set; }

    public long SNO { get; set; }
}
