using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class LabClassSub
{
    public string ClassName { get; set; } = null!;

    public string SubClassName { get; set; } = null!;

    public long Sno { get; set; }

    public long? SnoId { get; set; }
}
