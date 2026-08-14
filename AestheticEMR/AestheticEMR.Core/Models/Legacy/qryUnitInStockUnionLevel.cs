using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUnitInStockUnionLevel
{
    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemName { get; set; }

    public double? ReorderLevel { get; set; }

    public double? StockLevel { get; set; }

    [Column("Alert Status")]
    [StringLength(13)]
    [Unicode(false)]
    public string Alert_Status { get; set; } = null!;

    [StringLength(50)]
    public string? LocID { get; set; }
}
