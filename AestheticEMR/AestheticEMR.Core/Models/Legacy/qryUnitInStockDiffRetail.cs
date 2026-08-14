using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUnitInStockDiffRetail
{
    [StringLength(50)]
    public string ItemCode { get; set; } = null!;

    [StringLength(50)]
    public string ItemName { get; set; } = null!;

    public double UnitsInStock { get; set; }

    public double? ReorderLevel { get; set; }

    public double? UnitDiff { get; set; }
}
