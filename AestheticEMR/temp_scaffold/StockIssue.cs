using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockIssue
{
    public long IssueId { get; set; }

    public DateTime IssueDate { get; set; }

    public string? ItemId { get; set; }

    public string? Category { get; set; }

    public string? BatchNo { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public decimal Qty { get; set; }

    public decimal? StockQtyIn { get; set; }

    public string IssuedBy { get; set; } = null!;

    public string? Poid { get; set; }

    public string? Comments { get; set; }

    public string? InvType { get; set; }

    public bool? AttendedTo { get; set; }

    public bool? Suppres { get; set; }

    public string? ReverseId { get; set; }

    public string? Reversal { get; set; }

    public string? LocId { get; set; }

    public decimal? PrevBal { get; set; }

    public string? Drgcode { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? IsPost { get; set; }
}
