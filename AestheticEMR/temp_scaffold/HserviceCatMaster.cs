using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HserviceCatMaster
{
    public string MasterCatName { get; set; } = null!;

    public string? Clinic { get; set; }

    public string? RptHead { get; set; }
}
