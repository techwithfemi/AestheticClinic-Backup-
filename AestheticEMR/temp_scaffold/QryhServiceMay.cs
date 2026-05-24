using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class QryhServiceMay
{
    public string ServiceId { get; set; } = null!;

    public string? Service { get; set; }

    public string Category { get; set; } = null!;

    public decimal Private { get; set; }
}
