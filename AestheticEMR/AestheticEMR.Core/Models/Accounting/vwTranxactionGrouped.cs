using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwTranxactionGrouped
{
    public string AccountID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string? CoyID { get; set; }

    public decimal? Amount { get; set; }
}
