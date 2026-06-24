using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class TranxactionArchiveMonitor
{
    public long SNo { get; set; }

    public string BatchNo { get; set; } = null!;

    public string BatchName { get; set; } = null!;

    public string BatchCat { get; set; } = null!;

    public string AcctToReconcile { get; set; } = null!;

    public bool IsDone { get; set; }
}
