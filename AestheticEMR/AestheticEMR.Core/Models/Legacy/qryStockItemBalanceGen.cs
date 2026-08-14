using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockItemBalanceGen
{
    [StringLength(250)]
    public string CategoryName { get; set; } = null!;

    [StringLength(350)]
    public string ItemID { get; set; } = null!;

    [StringLength(350)]
    public string ItemName { get; set; } = null!;

    public double? ReOrderLevel { get; set; }

    public double? UnitsInStock { get; set; }
}
