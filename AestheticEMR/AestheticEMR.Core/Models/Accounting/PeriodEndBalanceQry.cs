using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class PeriodEndBalanceQry
{
    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;
}
