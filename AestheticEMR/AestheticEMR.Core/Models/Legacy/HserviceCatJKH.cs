using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("HserviceCatJKH")]
public partial class HserviceCatJKH
{
    [StringLength(100)]
    public string CatName { get; set; } = null!;

    [StringLength(100)]
    public string? Clinic { get; set; }
}
