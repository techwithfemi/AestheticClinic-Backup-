using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUsersAndRole
{
    public string? RoleId { get; set; }

    public string? LoginRole { get; set; }

    public string? UserName { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Password { get; set; }

    public string? AccountStatus { get; set; }

    public string? BranchCode { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public string? AppType { get; set; }

    public string? Clinic { get; set; }

    public string? UserLevel { get; set; }

    public string EmpId { get; set; } = null!;

    public string DesId { get; set; } = null!;

    public string Designation { get; set; } = null!;
}
