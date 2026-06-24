using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGLforRpt
{
    public long? SNo { get; set; }

    public long SNoCOA { get; set; }

    public short Serial { get; set; }

    public string? TranID { get; set; }

    public DateTime? TranDate { get; set; }

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal DrCr { get; set; }

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public decimal Debit { get; set; }

    public decimal? Credit { get; set; }

    public decimal LedgerDebit { get; set; }

    public decimal? LedgerCredit { get; set; }

    public decimal LedgerOpBal { get; set; }

    public decimal LedgerClBal { get; set; }

    public decimal? LedgerBalance { get; set; }

    public string? Description { get; set; }

    public string LedgerCode { get; set; } = null!;

    public string Ledger { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string? Periodval { get; set; }
}
