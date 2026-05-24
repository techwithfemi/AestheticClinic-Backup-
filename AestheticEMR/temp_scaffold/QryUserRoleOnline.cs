using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryUserRoleOnline
{
    public string UserName { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? AccountStatus { get; set; }

    public string RoleId { get; set; } = null!;

    public string? LoginRole { get; set; }

    public string? RoleName { get; set; }

    public bool? Enabled { get; set; }
}
