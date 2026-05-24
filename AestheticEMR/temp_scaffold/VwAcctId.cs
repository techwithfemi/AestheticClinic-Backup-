using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAcctId
{
    public string AcctType { get; set; } = null!;

    public int? IdMax { get; set; }
}
