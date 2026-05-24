using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhRetainershipUseTariff
{
    public string CoyId { get; set; } = null!;

    public string Company { get; set; } = null!;

    public string? UseCoyId { get; set; }

    public string? UseName { get; set; }

    public string? UseTariff { get; set; }

    public double? Pcent { get; set; }

    public string? Type { get; set; }

    public string? Remarks { get; set; }
}
