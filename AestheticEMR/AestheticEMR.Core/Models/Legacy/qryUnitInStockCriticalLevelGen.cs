using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUnitInStockCriticalLevelGen
{
    [StringLength(350)]
    public string ItemID { get; set; } = null!;

    public double? BulkUnit { get; set; }

    [StringLength(350)]
    public string ItemName { get; set; } = null!;

    public double? ReOrderLevel { get; set; }

    public double? CriticalLevel { get; set; }
}
