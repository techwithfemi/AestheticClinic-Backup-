using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockPositionInMainStore3
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? OpeningBalance { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Entry { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? ISSUE { get; set; }

    public double? Cost { get; set; }
}
