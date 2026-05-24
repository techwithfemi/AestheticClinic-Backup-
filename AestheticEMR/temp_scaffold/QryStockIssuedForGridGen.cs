using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryStockIssuedForGridGen
{
    public int IssueId { get; set; }

    public DateTime IssueDate { get; set; }

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int QtyIssued { get; set; }

    public double PharmacyQty { get; set; }

    public string? IssuedBy { get; set; }

    public string? Comments { get; set; }

    public string? ReverseId { get; set; }

    public string? Reversal { get; set; }
}
