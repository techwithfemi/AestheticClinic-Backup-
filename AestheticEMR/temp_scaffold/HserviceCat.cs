using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HserviceCat
{
    public string? MasterCatName { get; set; }

    public string CatName { get; set; } = null!;

    public string? Clinic { get; set; }

    public string? RptHead { get; set; }

    public long Sno { get; set; }
}
