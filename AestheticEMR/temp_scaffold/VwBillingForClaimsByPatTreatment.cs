using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingForClaimsByPatTreatment
{
    public int Sno { get; set; }

    public double? SubTotal { get; set; }

    public string BillNo { get; set; } = null!;

    public string Service { get; set; } = null!;
}
