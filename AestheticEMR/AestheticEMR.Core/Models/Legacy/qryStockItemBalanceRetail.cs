using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockItemBalanceRetail
{
    [StringLength(250)]
    public string? CategoryName { get; set; }

    [StringLength(50)]
    public string ItemID { get; set; } = null!;

    [StringLength(50)]
    public string ItemName { get; set; } = null!;

    public double? ReorderLevel { get; set; }

    public double UnitsInStock { get; set; }
}
