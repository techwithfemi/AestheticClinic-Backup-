using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AestheticEMR.Core;

[Keyless]
public partial class vwBillingForClaimsRemark
{
    public int Sno { get; set; }

    [StringLength(13)]
    [Unicode(false)]
    public string service { get; set; } = null!;

    [StringLength(17)]
    [Unicode(false)]
    public string BilltRemarks { get; set; } = null!;
}
