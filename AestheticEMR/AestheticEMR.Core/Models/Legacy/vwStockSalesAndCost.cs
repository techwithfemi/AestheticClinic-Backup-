using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockSalesAndCost
{
    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string drgName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Subtotal { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? COGS { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? Sales { get; set; }
}
