using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryHmolist
{
    public string RetainId { get; set; } = null!;

    public string? ClientCatId { get; set; }

    public string RetainName { get; set; } = null!;
}
