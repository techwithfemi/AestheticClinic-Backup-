using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwAccountsInfoBalSheet
{
    public long SNo { get; set; }

    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string? AccountDesc { get; set; }

    public string GroupName { get; set; } = null!;

    public long GroupMin { get; set; }

    public long GroupMax { get; set; }

    public decimal AccountOpAmt { get; set; }

    public decimal AccountClAmt { get; set; }

    public string? AccountCat { get; set; }

    public string? Remarks { get; set; }

    public string CatID { get; set; } = null!;
}
