using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwMarginAnalysis
{
    [StringLength(50)]
    public string? RetainID { get; set; }

    public int? Mth { get; set; }

    public int? Yr { get; set; }

    public double? Amount { get; set; }

    [StringLength(35)]
    public string? Period { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string? PeriodVal { get; set; }

    [StringLength(150)]
    public string Company { get; set; } = null!;
}
