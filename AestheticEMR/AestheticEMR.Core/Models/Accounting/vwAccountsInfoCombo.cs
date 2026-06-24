using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwAccountsInfoCombo
{
    public long SNo { get; set; }

    public string AccountNo { get; set; } = null!;

    public string AccountID { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? Remarks { get; set; }

    public bool? HiddenGp { get; set; }
}
