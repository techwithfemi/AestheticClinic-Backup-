using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDebtInfoForPat
{
    [StringLength(50)]
    public string? clientCat { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? Debt { get; set; }
}
