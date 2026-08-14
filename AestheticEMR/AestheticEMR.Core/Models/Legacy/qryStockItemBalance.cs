using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryStockItemBalance
{
    [StringLength(255)]
    [Unicode(false)]
    public string? CategoryName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ItemID { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ReOrderLevel { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitsInStock { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string LocID { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitCost { get; set; }

    [StringLength(43)]
    public string? QtyPerUnit { get; set; }

    [Column(TypeName = "decimal(37, 4)")]
    public decimal? Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? DeptID { get; set; }
}
