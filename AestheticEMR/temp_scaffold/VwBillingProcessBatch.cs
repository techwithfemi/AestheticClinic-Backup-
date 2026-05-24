using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingProcessBatch
{
    public long Id { get; set; }

    public DateTime AttdDate { get; set; }

    public string BillNo { get; set; } = null!;

    public string? Mth { get; set; }

    public string? Yr { get; set; }

    public string? BatchVal { get; set; }

    public string? BatchNo { get; set; }
}
