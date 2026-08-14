using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwAttendanceSumm
{
    [StringLength(2)]
    public string? Mth { get; set; }

    [StringLength(30)]
    public string? Yr { get; set; }

    [StringLength(30)]
    public string? MonthNAme { get; set; }

    public int? Num { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime recDate { get; set; }
}
