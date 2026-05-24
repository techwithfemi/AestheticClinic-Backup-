using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class RolesOnline
{
    public long Sno { get; set; }

    public string RoleId { get; set; } = null!;

    public string? LoginRole { get; set; }

    public bool? Enabled { get; set; }
}
