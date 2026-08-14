using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockEntryWtAvg
{
    [StringLength(350)]
    public string? ItemID { get; set; }

    [StringLength(150)]
    public string? Category { get; set; }

    public double? WtAvg { get; set; }

    [StringLength(50)]
    public string? LocID { get; set; }
}
