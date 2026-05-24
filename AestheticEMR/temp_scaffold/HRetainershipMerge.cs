using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HRetainershipMerge
{
    public string? Pno { get; set; }

    public string MergeFrom { get; set; } = null!;

    public string MergeTo { get; set; } = null!;
}
