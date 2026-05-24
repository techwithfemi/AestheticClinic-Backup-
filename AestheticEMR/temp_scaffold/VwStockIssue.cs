using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockIssue
{
    public int IssueId { get; set; }

    public DateTime IssueDate { get; set; }

    public string ItemId { get; set; } = null!;

    public int Qty { get; set; }

    public string IssuedBy { get; set; } = null!;

    public string EmpFullname { get; set; } = null!;

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public string? Category { get; set; }
}
