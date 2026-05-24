using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Tranxaction
{
    public long Sno { get; set; }

    public DateTime EntryDate { get; set; }

    public string? Pno { get; set; }

    public string BillNo { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal RunningTotal { get; set; }

    public string? Remarks { get; set; }

    public bool? IsRev { get; set; }

    public int Seed { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? TranStatus { get; set; }
}
