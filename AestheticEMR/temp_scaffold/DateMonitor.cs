using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DateMonitor
{
    public DateTime DtBill { get; set; }

    public DateTime? DtOthers { get; set; }

    public byte[] LastUpdate { get; set; } = null!;
}
