using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillingDetail
{
    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [Column(TypeName = "decimal(38, 2)")]
    public decimal? subTotal { get; set; }
}
