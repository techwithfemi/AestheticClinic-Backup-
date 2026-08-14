using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockBalancePeriodEnd")]
public partial class StockBalancePeriodEnd
{
    public long SNo { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitsInStock { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Period { get; set; } = null!;

    public DateOnly EntryDate { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? LocID { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitLevel { get; set; }
}
