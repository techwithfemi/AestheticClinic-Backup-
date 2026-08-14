using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingForClaimsByPatTreatment
{
    public int Sno { get; set; }

    public double? SubTotal { get; set; }

    [StringLength(50)]
    public string billNO { get; set; } = null!;

    [StringLength(17)]
    [Unicode(false)]
    public string Service { get; set; } = null!;
}
