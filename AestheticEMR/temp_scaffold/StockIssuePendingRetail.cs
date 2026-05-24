using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockIssuePendingRetail
{
    public int IssueId { get; set; }

    public DateTime EntryDate { get; set; }

    public string ItemId { get; set; } = null!;

    public int Qty { get; set; }

    public string EnteredBy { get; set; } = null!;

    public string? CustId { get; set; }

    public string? Comments { get; set; }

    public DateTime? ExpectedDate { get; set; }
}
