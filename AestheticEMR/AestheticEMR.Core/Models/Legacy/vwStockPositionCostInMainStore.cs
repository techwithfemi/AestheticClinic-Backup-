using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockPositionCostInMainStore
{
    [StringLength(255)]
    [Unicode(false)]
    public string? ItemID { get; set; }

    public double? Cost { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }
}
