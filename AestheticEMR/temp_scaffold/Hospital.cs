using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Hospital
{
    public string GroupCode { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? Address { get; set; }
}
