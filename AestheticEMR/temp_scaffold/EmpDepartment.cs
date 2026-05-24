using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class EmpDepartment
{
    public string DeptId { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public string? DeptAddress { get; set; }
}
