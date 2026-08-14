using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingProcessDetailsGroupedVerify
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    public double? Subtotal { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal AmountBilled { get; set; }

    public double? Diff { get; set; }

    [StringLength(50)]
    public string pNO { get; set; } = null!;
}
