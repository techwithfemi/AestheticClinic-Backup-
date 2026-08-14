using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillHeadingUnion
{
    [StringLength(500)]
    public string? RptHead { get; set; }

    [StringLength(350)]
    public string Category { get; set; } = null!;

    [StringLength(550)]
    [Unicode(false)]
    public string? ItemName { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string BillType { get; set; } = null!;
}
