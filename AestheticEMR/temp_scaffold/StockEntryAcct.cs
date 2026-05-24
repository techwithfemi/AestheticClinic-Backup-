using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class StockEntryAcct
{
    public long Sno { get; set; }

    public DateTime EntryDate { get; set; }

    public string Poid { get; set; } = null!;

    public decimal Amount { get; set; }

    public bool IsPost { get; set; }

    public string SuppAcctId { get; set; } = null!;

    public long SuppId { get; set; }

    public string? InvoiceNo { get; set; }

    public string? InvAcctNo { get; set; }

    public DateTime? PostDate { get; set; }

    public DateTime? EntryDate2 { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? Suppres { get; set; }

    public bool? AttendedToByVouch { get; set; }
}
