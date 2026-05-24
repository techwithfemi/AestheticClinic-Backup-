using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwRole
{
    public string RoleId { get; set; } = null!;

    public string? LoginRole { get; set; }

    public string? Enabled { get; set; }
}
