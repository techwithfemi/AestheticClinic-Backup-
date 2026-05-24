using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillingBalanceDueForPrivateGroupedOld
{
    public string PNo { get; set; } = null!;

    public string? ClientId { get; set; }

    public decimal? TotDue { get; set; }

    public string PCatId { get; set; } = null!;
}
