using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class AppDefault
{
    public string Id { get; set; } = null!;

    public string Idval { get; set; } = null!;

    public string? Remarks { get; set; }

    public bool? Editable { get; set; }
}
