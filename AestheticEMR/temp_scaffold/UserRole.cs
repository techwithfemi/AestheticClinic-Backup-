using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class UserRole
{
    public string Username { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public long Sno { get; set; }
}
