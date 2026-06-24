using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxDebitCreditGroupedByPeriod
{
    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public decimal? Amount { get; set; }

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }
}
