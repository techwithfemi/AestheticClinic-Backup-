using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class hScanTemplate
{
    public long SNo { get; set; }

    [StringLength(1000)]
    public string Category { get; set; } = null!;

    public string Details { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? InvType { get; set; }
}
