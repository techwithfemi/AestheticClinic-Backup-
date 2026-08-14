using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockEntryCost
{
    public long EntryID { get; set; }

    [StringLength(350)]
    public string? ItemID { get; set; }

    [StringLength(150)]
    public string? Category { get; set; }

    public double? Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }
}
