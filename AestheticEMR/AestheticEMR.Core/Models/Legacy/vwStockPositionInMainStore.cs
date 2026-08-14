using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockPositionInMainStore
{
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Store { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Pharmacy { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Issue { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? Entry { get; set; }
}
