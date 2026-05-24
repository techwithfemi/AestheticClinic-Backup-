using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HScanPix
{
    public long Sno { get; set; }

    public string ScanDesc { get; set; } = null!;

    public string PixCode { get; set; } = null!;
}
