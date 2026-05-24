using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class Company
{
    public string CoyId { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? CoyLocation { get; set; }
}
