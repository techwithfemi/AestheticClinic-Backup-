using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class TranxactionJournalExternal
{
    public long SNo { get; set; }

    public string TranID { get; set; } = null!;

    public string AccountID { get; set; } = null!;

    public string TranNo { get; set; } = null!;

    public DateTime TranDate { get; set; }

    public string CostCenterID { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string TranCat { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Period { get; set; } = null!;

    public DateTime? Prd2 { get; set; }

    public string UserName { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? CoyID { get; set; }

    public decimal? AcctBal { get; set; }

    public bool? isPost { get; set; }

    public bool? hideInRpt { get; set; }

    public DateTime EntryDate2 { get; set; }

    public DateTime EntryTime { get; set; }

    public string AppName { get; set; } = null!;

    public string ClientName { get; set; } = null!;
}
