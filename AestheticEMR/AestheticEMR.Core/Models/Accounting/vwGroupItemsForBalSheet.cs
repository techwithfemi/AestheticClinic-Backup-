using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGroupItemsForBalSheet
{
    public string GroupID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public bool? HiddenGp { get; set; }
}
