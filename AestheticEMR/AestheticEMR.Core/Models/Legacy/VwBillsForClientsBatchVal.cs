using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class VwBillsForClientsBatchVal
{
    public string? BatchVal { get; set; }

    public string? CoyCode { get; set; }

    public string? BatchNo { get; set; }

    public string InvNo { get; set; } = null!;
}
