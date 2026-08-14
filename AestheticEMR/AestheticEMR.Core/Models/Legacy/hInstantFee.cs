using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
[Table("hInstantFee")]
public partial class hInstantFee
{
    public long SNo { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string ItemName { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ItemType { get; set; } = null!;
}
