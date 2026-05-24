using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HRecordsMonitorForRpt
{
    public DateTime? Date { get; set; }

    public string? Description { get; set; }

    public int NumCount { get; set; }

    public string? Remarks { get; set; }
}
