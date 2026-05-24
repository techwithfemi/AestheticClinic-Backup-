using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockPositionIssue
{
    public int? IssueId { get; set; }

    public DateTime? IssueDate { get; set; }

    public string? ItemId { get; set; }

    public string? Drug { get; set; }

    public int? Qty { get; set; }

    public string? IssuedBy { get; set; }

    public string? Comments { get; set; }

    public string? Category { get; set; }

    public double? DrugPriceLast { get; set; }

    public bool? Suppres { get; set; }

    public double? BulkUnit { get; set; }

    public double? PrevBal { get; set; }

    public string? LocId { get; set; }

    public string? Poid { get; set; }

    public string? Expr1 { get; set; }
}
