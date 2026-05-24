using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockIssueGen
{
    public int IssueId { get; set; }

    public DateTime IssueDate { get; set; }

    public string ItemId { get; set; } = null!;

    public string? Category { get; set; }

    public string? BatchNo { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public int Qty { get; set; }

    public int? StockQtyIn { get; set; }

    public string IssuedBy { get; set; } = null!;

    public string? Poid { get; set; }

    public string? Comments { get; set; }

    public string? InvType { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public string? ReverseId { get; set; }

    public string? Reversal { get; set; }
}
