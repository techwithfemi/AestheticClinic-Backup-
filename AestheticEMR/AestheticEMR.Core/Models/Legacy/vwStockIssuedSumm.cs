using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwStockIssuedSumm
{
    [StringLength(50)]
    public string? POID { get; set; }

    public int QtyUsed { get; set; }

    [StringLength(50)]
    public string ItemID { get; set; } = null!;

    [StringLength(50)]
    public string? Category { get; set; }
}
