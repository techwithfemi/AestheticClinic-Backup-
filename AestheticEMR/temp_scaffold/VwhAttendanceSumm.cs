using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhAttendanceSumm
{
    public string ItemName { get; set; } = null!;

    public long? NumVal { get; set; }

    public DateTime Dtdate { get; set; }

    public string? Yr { get; set; }
}
