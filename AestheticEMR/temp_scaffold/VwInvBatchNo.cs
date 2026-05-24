using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwInvBatchNo
{
    public DateTime InvDate { get; set; }

    public string? BatchNo { get; set; }

    public string? CoyCode { get; set; }

    public string RetainName { get; set; } = null!;
}
