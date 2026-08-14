using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryhDispensingForRptGrouped
{
    [StringLength(1000)]
    public string drgName { get; set; } = null!;

    [StringLength(500)]
    public string drgCatName { get; set; } = null!;

    public double? Qty { get; set; }

    public double? Price { get; set; }

    public double? Amount { get; set; }

    public double? Cost { get; set; }
}
