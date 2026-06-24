using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwAccountMasterInfo
{
    public long SNo { get; set; }

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string AccountDesc { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public long GroupMin { get; set; }

    public long GroupMax { get; set; }

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public string? AccountCat { get; set; }

    public string? Remarks { get; set; }

    public string CatID { get; set; } = null!;

    public bool? HiddenGp { get; set; }

    public string CatName { get; set; } = null!;

    public string Expr1 { get; set; } = null!;

    public bool? isDummy { get; set; }

    public bool? Hidden { get; set; }

    public long CatMasterMin { get; set; }

    public long CatMasterMax { get; set; }

    public string? Period { get; set; }

    public string CoyID { get; set; } = null!;

    public bool isContra { get; set; }

    public bool? isPerm { get; set; }

    public string? BalStatusMaster { get; set; }

    public string? BalStatus { get; set; }
}
