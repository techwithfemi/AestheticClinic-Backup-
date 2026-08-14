using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingProcessGrouped
{
    [StringLength(50)]
    public string? RetainCode { get; set; }

    [Column(TypeName = "decimal(38, 0)")]
    public decimal? AmountBilled { get; set; }
}
