using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwReportHeader
{
    public string Description { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string? RptType { get; set; }

    public string? Remarks { get; set; }

    public bool? Suppres { get; set; }

    public string? RptLevel { get; set; }

    public short? RptSerial { get; set; }

    public string? RptTitle { get; set; }
}
