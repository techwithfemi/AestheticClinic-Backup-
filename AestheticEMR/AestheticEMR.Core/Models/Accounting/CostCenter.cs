using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class CostCenter
{
    public long SNo { get; set; }

    public string CenterID { get; set; } = null!;

    public string CenterName { get; set; } = null!;

    public string DeptID { get; set; } = null!;
}
