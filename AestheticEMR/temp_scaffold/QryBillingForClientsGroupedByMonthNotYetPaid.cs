using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingForClientsGroupedByMonthNotYetPaid
{
    public string BillingMonth { get; set; } = null!;

    public int BillingYear { get; set; }

    public string ClientId { get; set; } = null!;

    public string Clientname { get; set; } = null!;

    public decimal? Amount { get; set; }
}
