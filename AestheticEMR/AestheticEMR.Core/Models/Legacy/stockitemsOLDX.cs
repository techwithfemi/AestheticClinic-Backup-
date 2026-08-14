using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("stockitemsOLDX")]
public partial class stockitemsOLDX
{
    [StringLength(50)]
    public string ItemID { get; set; } = null!;

    [StringLength(50)]
    public string ItemName { get; set; } = null!;

    [StringLength(50)]
    public string ItemCatID { get; set; } = null!;

    [StringLength(50)]
    public string? QuantityPerUnit { get; set; }

    public double? UnitPrice { get; set; }

    public double UnitsInStock { get; set; }

    public double? ReorderLevel { get; set; }

    public bool? Discontinued { get; set; }

    [StringLength(50)]
    public string? brand { get; set; }
}
