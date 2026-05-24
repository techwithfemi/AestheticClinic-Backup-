using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class TranxactionArchive
{
    public long Sno { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Pno { get; set; }

    public string? BillNo { get; set; }

    public decimal? Amount { get; set; }

    public decimal? RunningTotal { get; set; }

    public string? Remarks { get; set; }

    public bool? IsRev { get; set; }

    public int? Seed { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? TranStatus { get; set; }

    public bool? IsLatest { get; set; }

    public long SnoId { get; set; }

    public DateTime? EntryDate2 { get; set; }

    public DateTime? EntryTime2 { get; set; }

    public string? ClientName2 { get; set; }

    public string? AppName2 { get; set; }
}
