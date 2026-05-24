using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAcctOpenBal
{
    public string? AcctId { get; set; }

    public string AcctName { get; set; } = null!;

    public string AcctType { get; set; } = null!;
}
