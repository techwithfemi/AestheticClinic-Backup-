using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryUnitInStockRegularLevelRetail
{
    [StringLength(250)]
    public string ItemID { get; set; } = null!;

    [StringLength(250)]
    public string ItemName { get; set; } = null!;

    public double? UnitsInStock { get; set; }

    public double? ReOrderLevel { get; set; }

    public double? RegularLevel { get; set; }
}
