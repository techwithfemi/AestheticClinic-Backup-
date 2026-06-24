using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class TranxactionJournalTemp
{
    public long SNo { get; set; }

    public DateTime TranDate { get; set; }

    public string? TranID { get; set; }

    public string AccountDebit { get; set; } = null!;

    public string AccountCredit { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string TranCat { get; set; } = null!;

    public bool IsPost { get; set; }

    public string? Remarks { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public DateTime EntryTime { get; set; }

    public string AppName { get; set; } = null!;

    public string ClientName { get; set; } = null!;
}
