using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwAcctName
{
    public long Sno { get; set; }

    public string AcctId { get; set; } = null!;

    public string AcctName { get; set; } = null!;

    public string AcctType { get; set; } = null!;

    public string? Remarks { get; set; }

    public string AcctStatus { get; set; } = null!;
}
