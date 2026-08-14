using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUnitInStockDiffGen
{
    [StringLength(150)]
    public string Category { get; set; } = null!;

    [StringLength(350)]
    public string ItemCode { get; set; } = null!;

    [StringLength(350)]
    public string ItemName { get; set; } = null!;

    public double BulkUnit { get; set; }

    public double? ReOrderLevel { get; set; }

    public double UnitDiff { get; set; }
}
