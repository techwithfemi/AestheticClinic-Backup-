using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AcctPeriodTypesDetail
{
    public long SNo { get; set; }

    public string PrdType { get; set; } = null!;

    public short Mth { get; set; }

    public short FinPrd { get; set; }

    public string? FinPrd2 { get; set; }
}
