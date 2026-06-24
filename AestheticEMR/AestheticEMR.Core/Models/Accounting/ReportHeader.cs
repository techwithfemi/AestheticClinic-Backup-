using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class ReportHeader
{
    public long SNo { get; set; }

    public string Description { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string? CatID { get; set; }

    public byte? RptLevel { get; set; }

    public decimal Amount { get; set; }

    public string? RptType { get; set; }

    public string? Remarks { get; set; }

    public bool? Suppres { get; set; }
}
