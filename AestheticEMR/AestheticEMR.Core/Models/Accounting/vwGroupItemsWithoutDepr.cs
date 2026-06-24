using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGroupItemsWithoutDepr
{
    public string GroupID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public bool? HiddenGp { get; set; }

    public bool? Suppres { get; set; }
}
