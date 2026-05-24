using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillingTariff
{
    public string RetainId { get; set; } = null!;

    public string RetainName { get; set; } = null!;

    public string? MapTo { get; set; }

    public string? ClientCatId { get; set; }
}
