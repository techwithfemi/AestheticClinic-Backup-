using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTranxBalance
{
    public DateTime TranDate { get; set; }

    public string AcctName { get; set; } = null!;

    public string? AcctGp { get; set; }

    public string DrCr { get; set; } = null!;

    public double? Balance { get; set; }
}
