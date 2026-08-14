using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryPaymentsGrouped
{
    [StringLength(50)]
    public string pNO { get; set; } = null!;

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "numeric(38, 0)")]
    public decimal? SumOfAmountPaid { get; set; }
}
