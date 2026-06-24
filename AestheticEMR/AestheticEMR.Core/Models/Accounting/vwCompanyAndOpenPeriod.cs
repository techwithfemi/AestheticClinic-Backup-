using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwCompanyAndOpenPeriod
{
    public string CoyID { get; set; } = null!;

    public string Coyname { get; set; } = null!;

    public string? Period { get; set; }

    public bool isClose { get; set; }

    public DateTime AcctMonth { get; set; }

    public string? PeriodVal { get; set; }

    public DateTime PrdClose { get; set; }
}
