using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGLforRpt_SelfJoin
{
    public short Serial { get; set; }

    public DateTime TranDate { get; set; }

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public decimal Debit { get; set; }

    public decimal? Credit { get; set; }

    public string? DebitDescription { get; set; }

    public string? CreditDescription { get; set; }

    public string LedgerCode { get; set; } = null!;

    public string Ledger { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;
}
