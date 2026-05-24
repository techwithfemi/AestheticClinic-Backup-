using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class ClinicTime
{
    public long Sno { get; set; }

    public string TimeOfDay { get; set; } = null!;
}
