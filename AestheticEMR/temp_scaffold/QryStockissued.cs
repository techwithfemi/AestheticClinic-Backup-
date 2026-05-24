using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockissued
{
    public long IssueId { get; set; }

    public DateTime IssueDate { get; set; }

    public string? ItemId { get; set; }

    public string? ItemName { get; set; }

    public decimal Qty { get; set; }

    public string? Comments { get; set; }

    public string? IssuedBy { get; set; }

    public string? Category { get; set; }

    public string? LocId { get; set; }

    public decimal? PrevBal { get; set; }

    public string? QtyPerUnit { get; set; }

    public string? DeptId { get; set; }
}
