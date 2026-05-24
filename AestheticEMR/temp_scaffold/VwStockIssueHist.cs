using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockIssueHist
{
    public string ItemId { get; set; } = null!;

    public string? Category { get; set; }

    public string? Locid { get; set; }

    public DateTime IssueDate { get; set; }

    public int? Qty { get; set; }
}
