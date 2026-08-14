using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("TariffSpecial")]
public partial class TariffSpecial
{
    public long SNo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ItemCode { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Qty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Remarks { get; set; }
}
