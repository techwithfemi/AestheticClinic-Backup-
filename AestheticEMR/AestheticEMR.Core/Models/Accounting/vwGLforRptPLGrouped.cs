using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGLforRptPLGrouped
{
    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public short Serial { get; set; }

    public string LedgerCode { get; set; } = null!;

    public int? LedgerDebit { get; set; }

    public int? LedgerCredit { get; set; }

    public decimal? LedgerOpBal { get; set; }

    public decimal? LedgerClBal { get; set; }

    public decimal? LedgerBalance { get; set; }

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string? Periodval { get; set; }
}
