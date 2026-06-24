using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwBranchDept
{
    public long SNo { get; set; }

    public string? DeptID { get; set; }

    public string DeptName { get; set; } = null!;

    public string DivName { get; set; } = null!;

    public string DivID { get; set; } = null!;
}
