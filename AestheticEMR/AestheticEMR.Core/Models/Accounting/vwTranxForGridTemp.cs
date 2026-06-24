using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxForGridTemp
{
    public DateTime TranDate { get; set; }

    public string AccountNameDebit { get; set; } = null!;

    public string AccountDebit { get; set; } = null!;

    public string AccountNameCredit { get; set; } = null!;

    public string AccountCredit { get; set; } = null!;

    public decimal Debit { get; set; }

    public decimal? Credit { get; set; }

    public string? Description { get; set; }

    public string? TranNo { get; set; }

    public DateTime EntryDate { get; set; }

    public string UserName { get; set; } = null!;

    public long SNo { get; set; }

    public string? Remarks { get; set; }

    public string? CoyID { get; set; }

    public bool IsPost { get; set; }

    public bool? isClose { get; set; }
}
