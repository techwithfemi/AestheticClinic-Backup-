using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGroupItemsForBalSheet2
{
    public string GroupID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string? CoyID { get; set; }
}
