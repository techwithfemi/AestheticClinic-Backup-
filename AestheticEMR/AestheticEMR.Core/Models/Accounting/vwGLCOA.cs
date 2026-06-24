using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwGLCOA
{
    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public string GroupID { get; set; } = null!;

    public string LedgerCode { get; set; } = null!;

    public string? Periodval { get; set; }

    public string GroupName { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;
}
