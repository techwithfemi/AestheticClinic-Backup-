using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwLocationsIP
{
    public long SNo { get; set; }

    public string LocIP { get; set; } = null!;

    public string LocName { get; set; } = null!;

    public string? Remarks { get; set; }
}
