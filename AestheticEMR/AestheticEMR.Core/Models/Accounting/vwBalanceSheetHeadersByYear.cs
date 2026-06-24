using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwBalanceSheetHeadersByYear
{
    public string? ItemName { get; set; }

    public decimal? Amount { get; set; }

    public string? RptType { get; set; }

    public int? rptSerial { get; set; }

    public short? rptLevel { get; set; }

    public string? Remarks { get; set; }

    public int RptIndent { get; set; }

    public string? PeriodVal { get; set; }

    public string? PeriodVal1 { get; set; }

    public string? PeriodVal2 { get; set; }

    public DateOnly? PeriodDate1 { get; set; }

    public DateOnly? PeriodDate2 { get; set; }

    public string? MonthYear { get; set; }

    public string? MonthYearLong { get; set; }

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public bool? isLatest { get; set; }
}
