using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwLabItemsForGrid
{
    [StringLength(255)]
    [Unicode(false)]
    public string LabItem { get; set; } = null!;

    [StringLength(550)]
    public string? Category { get; set; }

    public int qtyPerUnit { get; set; }

    [StringLength(3)]
    public string? Capitated { get; set; }

    [StringLength(50)]
    public string? RangeVal { get; set; }

    [StringLength(50)]
    public string? TestUnit { get; set; }

    [StringLength(150)]
    public string? Class { get; set; }

    [StringLength(5000)]
    [Unicode(false)]
    public string? Range { get; set; }

    [StringLength(50)]
    public string? Sample { get; set; }

    [StringLength(50)]
    public string RevenueType { get; set; } = null!;

    [StringLength(8000)]
    [Unicode(false)]
    public string? ResultTemplate { get; set; }

    [StringLength(520)]
    [Unicode(false)]
    public string SubClass { get; set; } = null!;

    [StringLength(520)]
    [Unicode(false)]
    public string ClassName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? StockItem { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string LabType { get; set; } = null!;
}
