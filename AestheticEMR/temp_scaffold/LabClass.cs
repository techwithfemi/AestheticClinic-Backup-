using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class LabClass
{
    public string ClassName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public long Sno { get; set; }
}
