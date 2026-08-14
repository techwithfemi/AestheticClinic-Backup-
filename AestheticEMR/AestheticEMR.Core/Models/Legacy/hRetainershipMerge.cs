using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hRetainershipMerge")]
public partial class hRetainershipMerge
{
    [StringLength(50)]
    public string? PNo { get; set; }

    [StringLength(50)]
    public string MergeFrom { get; set; } = null!;

    [StringLength(50)]
    public string MergeTo { get; set; } = null!;
}
