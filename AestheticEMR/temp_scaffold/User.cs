using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class User
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

    public string? SaltedPass { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public DateTime? LastUpdated { get; set; }

    public DateTime? LastLoginDate { get; set; }
}
