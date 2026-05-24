using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockissuedGen
{
    public int IssueId { get; set; }

    public DateTime IssueDate { get; set; }

    public string ItemId { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public int Qty { get; set; }

    public string? Comments { get; set; }

    public string? IssuedBy { get; set; }

    public string? Category { get; set; }
}
