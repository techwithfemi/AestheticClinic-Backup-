using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwServiceforGridNHI
{
    [StringLength(255)]
    public string? Service { get; set; }

    [StringLength(255)]
    public string? Category { get; set; }

    public double? Price { get; set; }

    [StringLength(255)]
    public string? Company { get; set; }

    [StringLength(255)]
    public string? Remarks { get; set; }
}
