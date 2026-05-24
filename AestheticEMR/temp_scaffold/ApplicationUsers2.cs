using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class ApplicationUsers2
{
    public string UserName { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Password { get; set; }

    public string? AccountStatus { get; set; }

    public string? BranchCode { get; set; }
}
