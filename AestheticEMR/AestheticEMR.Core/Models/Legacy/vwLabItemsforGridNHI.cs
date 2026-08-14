using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwLabItemsforGridNHI
{
    [StringLength(255)]
    public string LabItem { get; set; } = null!;

    [StringLength(255)]
    public string Category { get; set; } = null!;

    public double? Price { get; set; }

    [StringLength(255)]
    public string? Company { get; set; }

    [StringLength(255)]
    public string? Remarks { get; set; }
}
