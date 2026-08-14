using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwCOGSandSales_Lab
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime invDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? EntryDate { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    [StringLength(2000)]
    public string? sympItem { get; set; }

    public double? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    public double? Price { get; set; }

    public double? CostAmount { get; set; }

    public double? SalesAmount { get; set; }

    public bool? isPost { get; set; }

    public bool? Suppres { get; set; }

    public double? Margin { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TranID { get; set; }

    public long? ReversedPair { get; set; }

    [StringLength(2070)]
    public string? remarks { get; set; }

    public bool? Reversed { get; set; }
}
