using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class ChartOfAccounts_BeginBalance_From_Excel
{
    public long SNo { get; set; }

    public string CoyID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public decimal OPBal { get; set; }

    public bool isPost { get; set; }

    public string? AccountName { get; set; }

    public string? GroupID { get; set; }

    public DateTime? TranDate { get; set; }

    public string? TranID { get; set; }

    public string? Description { get; set; }

    public string? Period { get; set; }
}
