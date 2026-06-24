using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGroupItemsFixedAsset
{
    public long SNo { get; set; }

    public string GroupID { get; set; } = null!;

    public string GroupName { get; set; } = null!;
}
