using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class JournalTemp
{
    public long Sno { get; set; }

    public DateTime DtDate { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public long TableSno { get; set; }

    public string TableName { get; set; } = null!;

    public string AcctGp { get; set; } = null!;

    public bool AttendedTo { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public bool? Suppres { get; set; }
}
