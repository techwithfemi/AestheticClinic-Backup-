using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhAttendanceSummItem
{
    public string ItemName { get; set; } = null!;

    public long NumVal { get; set; }

    public DateTime DtDate { get; set; }
}
