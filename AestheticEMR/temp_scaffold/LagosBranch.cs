using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class LagosBranch
{
    public decimal SerialNo { get; set; }

    public string BranchCode { get; set; } = null!;

    public string? BranchName { get; set; }

    public string? Location { get; set; }
}
