using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class ReportSummary
{
    public long SNo { get; set; }

    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string? ItemName { get; set; }

    public string? PeriodVal { get; set; }

    public decimal? Amount { get; set; }

    public string? Remarks { get; set; }

    public bool? isClose { get; set; }

    public bool? isTransfer { get; set; }

    public short? rptLevel { get; set; }

    public string? RptType { get; set; }

    public int? rptSerial { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? AppName { get; set; }

    public string? ClientName { get; set; }

    public bool? isLatest { get; set; }
}
