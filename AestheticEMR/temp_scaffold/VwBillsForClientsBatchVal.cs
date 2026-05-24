using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillsForClientsBatchVal
{
    public string? BatchVal { get; set; }

    public string? CoyCode { get; set; }

    public string? BatchNo { get; set; }

    public string InvNo { get; set; } = null!;
}
