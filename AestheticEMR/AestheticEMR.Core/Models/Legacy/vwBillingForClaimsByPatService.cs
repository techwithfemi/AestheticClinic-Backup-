using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingForClaimsByPatService
{
    public int Sno { get; set; }

    public double? SubTotal { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(550)]
    public string Service { get; set; } = null!;
}
