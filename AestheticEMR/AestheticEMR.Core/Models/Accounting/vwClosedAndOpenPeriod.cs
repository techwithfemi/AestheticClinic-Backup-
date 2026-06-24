using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwClosedAndOpenPeriod
{
    public string CoyID { get; set; } = null!;

    public string? Period { get; set; }

    public string PeriodYr { get; set; } = null!;

    public string? PeriodVal { get; set; }

    public bool isClose { get; set; }

    public bool? Suppres { get; set; }

    public DateTime AcctMonth { get; set; }

    public DateTime PrdClose { get; set; }
}
