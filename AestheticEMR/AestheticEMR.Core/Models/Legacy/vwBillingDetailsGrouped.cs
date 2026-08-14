using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingDetailsGrouped
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [StringLength(250)]
    [Unicode(false)]
    public string? billType { get; set; }

    public double? Qty { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? SubTotal { get; set; }

    public double Price { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BillTo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [StringLength(50)]
    public string? revType { get; set; }

    [StringLength(250)]
    public string? Dosage { get; set; }

    public bool? Reversed { get; set; }
}
