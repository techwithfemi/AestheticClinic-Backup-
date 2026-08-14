using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("LabServiceNHISCopy")]
public partial class LabServiceNHISCopy
{
    [StringLength(255)]
    public string DrgName { get; set; } = null!;

    [StringLength(255)]
    public string DrgCatName { get; set; } = null!;

    public double? Price { get; set; }

    [StringLength(255)]
    public string? Company { get; set; }

    [StringLength(255)]
    public string? Remarks { get; set; }
}
