using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvBatchNo2
{
    public DateTime? InvDate { get; set; }

    public string? BatchNo { get; set; }

    public string? BatchNo2 { get; set; }

    public string CoyCode { get; set; } = null!;

    public string RetainName { get; set; } = null!;
}
