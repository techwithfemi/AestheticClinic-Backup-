using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGLforRptGrouped
{
    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public short Serial { get; set; }

    public string LedgerCode { get; set; } = null!;

    public string? LedgerCodeVal { get; set; }

    public string Ledger { get; set; } = null!;

    public decimal? AccountOpAmt { get; set; }

    public decimal? AccountClAmt { get; set; }

    public decimal? LedgerOpBal { get; set; }

    public decimal? LedgerClBal { get; set; }

    public decimal? LedgerBalance { get; set; }

    public decimal? LedgerDebit { get; set; }

    public decimal? LedgerCredit { get; set; }

    public decimal? Amount { get; set; }

    public decimal? DrCr { get; set; }

    public decimal? Debit { get; set; }

    public decimal? Credit { get; set; }

    public string GroupID { get; set; } = null!;

    public string? Periodval { get; set; }

    public string GroupName { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;
}
