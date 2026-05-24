using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAccountInfo
{
    public string? AcctId { get; set; }

    public string AcctName { get; set; } = null!;

    public string Remarks { get; set; } = null!;
}
