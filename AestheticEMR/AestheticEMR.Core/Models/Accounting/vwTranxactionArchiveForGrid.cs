using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxactionArchiveForGrid
{
    public long SNo { get; set; }

    public string TranID { get; set; } = null!;

    public string TranNo { get; set; } = null!;

    public DateTime TranDate { get; set; }

    public string AccountID { get; set; } = null!;

    public string? AccountName { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public string TranCat { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string Period { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? CoyID { get; set; }

    public bool? isClose { get; set; }

    public decimal? AcctBal { get; set; }

    public string BatchNo { get; set; } = null!;

    public string BatchName { get; set; } = null!;

    public string BatchCat { get; set; } = null!;

    public string AcctToReconcile { get; set; } = null!;

    public bool IsDone { get; set; }
}
