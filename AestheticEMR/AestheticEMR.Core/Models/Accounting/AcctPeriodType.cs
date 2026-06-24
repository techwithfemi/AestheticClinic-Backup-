using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AcctPeriodType
{
    public long SNo { get; set; }

    public string PrdType { get; set; } = null!;
}
