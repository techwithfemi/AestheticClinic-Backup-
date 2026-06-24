using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetDepartment
{
    public string? DptCode { get; set; }

    public string DptName { get; set; } = null!;

    public long SNo { get; set; }
}
