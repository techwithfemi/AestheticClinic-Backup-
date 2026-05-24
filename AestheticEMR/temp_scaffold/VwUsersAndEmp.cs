using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwUsersAndEmp
{
    public string EmpId { get; set; } = null!;

    public string? UserName { get; set; }

    public string EmpFullname { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public string DeptId { get; set; } = null!;

    public string DesId { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Password { get; set; }

    public string? AccountStatus { get; set; }

    public string? BranchCode { get; set; }

    public string? SaltedPass { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
}
