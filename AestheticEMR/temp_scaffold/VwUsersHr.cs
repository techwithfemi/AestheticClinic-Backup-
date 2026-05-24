using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwUsersHr
{
    public string UserName { get; set; } = null!;

    public string? Password { get; set; }

    public string? Fullname { get; set; }

    public string? AccountStatus { get; set; }

    public string? BranchCode { get; set; }

    public string? AppType { get; set; }

    public string? Clinic { get; set; }

    public string? UserLevel { get; set; }

    public string RoleId { get; set; } = null!;

    public string? LoginRole { get; set; }

    public string Hname { get; set; } = null!;

    public string EmpId { get; set; } = null!;

    public bool? Enabled { get; set; }

    public string? HospId { get; set; }

    public long Sno { get; set; }

    public string? Location { get; set; }

    public string? DeptId { get; set; }

    public string? DeptName { get; set; }
}
