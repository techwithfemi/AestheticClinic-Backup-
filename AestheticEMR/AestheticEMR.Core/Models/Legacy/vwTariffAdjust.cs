using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwTariffAdjust
{
    public long SNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OldPrice { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal NewPrice { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string RevType { get; set; } = null!;

    [StringLength(150)]
    public string Company { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string CoyName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string billNo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Category { get; set; }
}
