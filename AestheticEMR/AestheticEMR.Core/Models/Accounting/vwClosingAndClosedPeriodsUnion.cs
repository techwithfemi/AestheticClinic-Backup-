using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwClosingAndClosedPeriodsUnion
{
    public string CoyID { get; set; } = null!;

    public string? Period { get; set; }

    public string PeriodYr { get; set; } = null!;

    public string? PeriodVal { get; set; }
}
