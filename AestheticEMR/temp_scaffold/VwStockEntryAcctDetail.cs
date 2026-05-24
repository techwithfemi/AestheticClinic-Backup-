using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwStockEntryAcctDetail
{
    public long Sno { get; set; }

    public DateTime? Invoicedate { get; set; }

    public string? Poid { get; set; }

    public long SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? AcctDebit { get; set; }

    public string? AcctCredit { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Debt { get; set; }

    public string AccountName { get; set; } = null!;

    public string InvoiceNo { get; set; } = null!;

    public bool? IsPost { get; set; }

    public int? Yr { get; set; }

    public int? Mth { get; set; }

    public int? PostYr { get; set; }

    public int? PostMth { get; set; }

    public DateTime? EntryDate2 { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? Suppres { get; set; }

    public string? Remarks { get; set; }
}
