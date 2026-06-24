using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxGroupedByAccountID
{
    public string AccountName { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }

    public decimal? Balance { get; set; }
}
