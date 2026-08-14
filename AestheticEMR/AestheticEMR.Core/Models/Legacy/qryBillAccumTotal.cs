using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class qryBillAccumTotal
{
    [StringLength(50)]
    public string? PatNo { get; set; }

    [StringLength(50)]
    public string consultID { get; set; } = null!;

    public double? Total { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? billtype { get; set; }
}
