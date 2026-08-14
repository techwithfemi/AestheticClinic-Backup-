using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwMarginAnalysis2
{
    public int? Mth { get; set; }

    public int? Yr { get; set; }

    [StringLength(150)]
    public string? Company { get; set; }

    [StringLength(50)]
    public string? RetainID { get; set; }

    public double AmountCap { get; set; }

    public double AmountCost { get; set; }

    public double? Margin { get; set; }

    [StringLength(35)]
    public string? Period { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string? PeriodVal { get; set; }
}
