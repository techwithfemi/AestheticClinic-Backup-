using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwPOrderDetailsApprvEntryGroupedGen
{
    public long? ApprvID { get; set; }

    public int? Qty { get; set; }

    [StringLength(50)]
    public string ItemID { get; set; } = null!;
}
