using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillHeading
{
    public long Sno { get; set; }

    public string? HeadName { get; set; }

    public string? Remarks { get; set; }

    public bool? Suppres { get; set; }
}
