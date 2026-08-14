using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDrugsForGridStockUpdate
{
    [StringLength(350)]
    public string Drug { get; set; } = null!;

    [StringLength(50)]
    public string Category { get; set; } = null!;

    public double UnitsInStock { get; set; }

    public double? ReOrderLevel { get; set; }
}
