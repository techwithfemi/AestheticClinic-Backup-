using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwAccountsInfoGLforConfirm
{
    public string GroupID { get; set; } = null!;

    public decimal? Amount { get; set; }

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;
}
