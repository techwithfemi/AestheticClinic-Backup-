using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingDetail
{
    public string BillNo { get; set; } = null!;

    public decimal? SubTotal { get; set; }
}
