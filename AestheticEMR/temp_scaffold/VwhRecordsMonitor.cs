using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRecordsMonitor
{
    public long Sno { get; set; }

    public string Description { get; set; } = null!;

    public DateTime DtDate { get; set; }

    public int NumCount { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }
}
