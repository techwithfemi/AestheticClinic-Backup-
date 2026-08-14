using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillHeadingForService
{
    [StringLength(500)]
    public string? RptHead { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Category { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string? ItemName { get; set; }
}
