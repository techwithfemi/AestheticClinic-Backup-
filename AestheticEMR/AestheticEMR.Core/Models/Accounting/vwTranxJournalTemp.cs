using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxJournalTemp
{
    public long SNo { get; set; }

    public string? TranID { get; set; }

    public DateTime TranDate { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string TranCat { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string UserName { get; set; } = null!;

    public string? Remarks { get; set; }

    public string AccountNameDebit { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? CatName { get; set; }

    public string? CatName2 { get; set; }

    public string CoyID { get; set; } = null!;

    public string AccountDebit { get; set; } = null!;

    public string AccountCredit { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public DateTime EntryTime { get; set; }

    public bool IsPost { get; set; }

    public bool? isClose { get; set; }

    public string AccountNameCredit { get; set; } = null!;
}
