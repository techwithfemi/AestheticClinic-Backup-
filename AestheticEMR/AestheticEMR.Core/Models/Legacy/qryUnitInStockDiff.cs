using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUnitInStockDiff
{
    [StringLength(255)]
    [Unicode(false)]
    public string? ItemCode { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ItemName { get; set; }

    public double BulkUnit { get; set; }

    public double? ReOrderLevel { get; set; }

    public double UnitDiff { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }
}
