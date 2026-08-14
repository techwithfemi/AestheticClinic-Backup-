using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwDebtInfoForPatUnionGrouped
{
    [StringLength(50)]
    public string? pNO { get; set; }

    public double? Debt { get; set; }

    [StringLength(50)]
    public string? clientCat { get; set; }
}
