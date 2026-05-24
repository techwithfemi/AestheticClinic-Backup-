using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class UserInfo
{
    public string UserId { get; set; } = null!;

    public string? Username { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? City { get; set; }

    public string? Designation { get; set; }
}
