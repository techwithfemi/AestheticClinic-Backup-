using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwBanksAndCash
{
    public string AccountID { get; set; } = null!;

    public string AccountNo { get; set; } = null!;

    public string GroupID { get; set; } = null!;

    public string AccountName { get; set; } = null!;
}
