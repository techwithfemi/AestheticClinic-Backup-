using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryBillAccumTotal
{
    public string? PatNo { get; set; }

    public string ConsultId { get; set; } = null!;

    public double? Total { get; set; }

    public string? Billtype { get; set; }
}
