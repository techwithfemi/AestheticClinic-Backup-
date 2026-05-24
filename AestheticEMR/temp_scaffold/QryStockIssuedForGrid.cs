using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockIssuedForGrid
{
    public long IssueId { get; set; }

    public DateTime IssueDate { get; set; }

    public string? ItemCode { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Category { get; set; }

    public decimal QtyIssued { get; set; }

    public decimal? PharmacyQty { get; set; }

    public string? IssuedBy { get; set; }

    public string? Comments { get; set; }

    public string? ReverseId { get; set; }

    public string? Reversal { get; set; }
}
