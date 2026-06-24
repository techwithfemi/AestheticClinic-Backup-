using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwReportSummary2
{
    public long SNo { get; set; }

    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string? PeriodVal { get; set; }

    public string? ItemName { get; set; }

    public decimal Amount { get; set; }

    public string? Remarks { get; set; }

    public bool? isClose { get; set; }

    public bool? isTransfer { get; set; }

    public string? RptLevel { get; set; }

    public string? RptType { get; set; }

    public string GroupID { get; set; } = null!;

    public short? RptSerial { get; set; }

    public bool? isLatest { get; set; }
}
