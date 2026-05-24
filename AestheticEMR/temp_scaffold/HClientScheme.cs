using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HClientScheme
{
    public string ClientType { get; set; } = null!;

    public string? SchemeId { get; set; }

    public string? SchemeName { get; set; }
}
