using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwcheckPeriodExist
{
    public long SNo { get; set; }

    public DateTime TranDate { get; set; }

    public string Period { get; set; } = null!;

    public string? Remarks { get; set; }

    public bool isClose { get; set; }

    public DateTime StartDate { get; set; }

    public int MonthCounter { get; set; }

    public string PeriodYr { get; set; } = null!;

    public DateTime EndDate { get; set; }

    public string CoyID { get; set; } = null!;

    public bool? Suppres { get; set; }

    public string? PeriodVal { get; set; }
}
