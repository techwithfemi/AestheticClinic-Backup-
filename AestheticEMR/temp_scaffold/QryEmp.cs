using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryEmp
{
    public string EmpId { get; set; } = null!;

    public string EmpFullname { get; set; } = null!;

    public string DesId { get; set; } = null!;

    public string DesName { get; set; } = null!;

    public string DeptId { get; set; } = null!;

    public string DeptName { get; set; } = null!;
}
