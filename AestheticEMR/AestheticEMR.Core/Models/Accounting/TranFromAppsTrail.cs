using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class TranFromAppsTrail
{
    public long SNo { get; set; }

    public string TranDate { get; set; } = null!;

    public string TranID { get; set; } = null!;

    public string TranNoApp { get; set; } = null!;

    public string Remarks { get; set; } = null!;
}
