using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetSubGroupX
{
    public string GroupCode { get; set; } = null!;

    public string? SubGroupCode { get; set; }

    public string? SubGroupName { get; set; }

    public byte SNo { get; set; }
}
