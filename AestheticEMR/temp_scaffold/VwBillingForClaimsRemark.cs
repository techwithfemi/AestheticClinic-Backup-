using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingForClaimsRemark
{
    public int Sno { get; set; }

    public string Service { get; set; } = null!;

    public string BilltRemarks { get; set; } = null!;
}
