using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillsForClientsPosting
{
    public long Sno { get; set; }

    public string Company { get; set; } = null!;

    public string RetainCode { get; set; } = null!;

    public string? AcctId { get; set; }

    public double? Debt { get; set; }

    public string? DebtType { get; set; }

    public string InvNo { get; set; } = null!;

    public decimal? AmountInvoiced { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? AmtBf { get; set; }

    public string? BatchNo { get; set; }

    public bool? IsPost { get; set; }

    public bool? IsOld { get; set; }

    public string? Remarks { get; set; }

    public bool? AttendedToByClient { get; set; }

    public DateOnly? InvoiceDate { get; set; }

    public string? CoyCode { get; set; }

    public decimal? Balance { get; set; }
}
