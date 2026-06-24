using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwCostCenter
{
    public long SNo { get; set; }

    public string CenterName { get; set; } = null!;

    public string DeptID { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public string? CenterID { get; set; }

    public string? DivID { get; set; }

    public string DivName { get; set; } = null!;

    public string? CoyID { get; set; }

    public string? Coyname { get; set; }
}
