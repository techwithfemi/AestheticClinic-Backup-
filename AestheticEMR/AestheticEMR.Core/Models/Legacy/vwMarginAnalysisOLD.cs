using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwMarginAnalysisOLD
{
    [Column(TypeName = "datetime")]
    public DateTime BillDate { get; set; }

    [StringLength(2)]
    [Unicode(false)]
    public string? Mth { get; set; }

    public int? Yr { get; set; }

    public double Amount { get; set; }

    [StringLength(35)]
    public string? Period { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string? PeriodVal { get; set; }
}
