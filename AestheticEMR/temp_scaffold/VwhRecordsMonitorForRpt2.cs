using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRecordsMonitorForRpt2
{
    public long? Sno { get; set; }

    public DateTime Date { get; set; }

    public string Description { get; set; } = null!;

    public int NumCount { get; set; }

    public string? Remarks { get; set; }

    public DateTime? EntryDate { get; set; }
}
