using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOrderDetailsApprvEntryGrouped
{
    public long? ApprvID { get; set; }

    public double? Qty { get; set; }

    [StringLength(350)]
    public string? ItemID { get; set; }
}
