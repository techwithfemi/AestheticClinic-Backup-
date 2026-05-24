using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingForClientsGroupedByMonth
{
    public string BillingMonth { get; set; } = null!;

    public int BillingYear { get; set; }

    public string ClientId { get; set; } = null!;

    public decimal? SumOfAmountBilled { get; set; }
}
