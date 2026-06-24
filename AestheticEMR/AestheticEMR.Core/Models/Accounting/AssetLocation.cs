using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetLocation
{
    public string? LocationCode { get; set; }

    public string? LocationName { get; set; }

    public byte SNo { get; set; }
}
