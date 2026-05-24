using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class UsersArchive
{
    public string UserName { get; set; } = null!;

    public string EmpId { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Password { get; set; }

    public string? AccountStatus { get; set; }

    public string? BranchCode { get; set; }

    public string? AppType { get; set; }

    public string? Clinic { get; set; }

    public string? UserLevel { get; set; }

    public long Sno { get; set; }
}
