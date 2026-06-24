using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetSubGroup
{
    public string GroupCode { get; set; } = null!;

    public string SubGroupCode { get; set; } = null!;

    public string SubGroupName { get; set; } = null!;
}
