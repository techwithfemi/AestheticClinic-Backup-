using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("StockMovt")]
public partial class StockMovt
{
    public long SNo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime dtDate { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string DrgName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Remarks { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? UnitCost { get; set; }
}
