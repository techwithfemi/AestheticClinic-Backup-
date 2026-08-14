using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPaymentsSumm
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountBilled { get; set; }

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? AmountPaid { get; set; }
}
