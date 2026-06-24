using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class Department
{
    public string DeptID { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public string? DeptAddress { get; set; }

    public string? Location { get; set; }
}
