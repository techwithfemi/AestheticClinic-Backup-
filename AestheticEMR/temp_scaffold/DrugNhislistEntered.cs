using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugNhislistEntered
{
    public string CoyId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? UseTariff { get; set; }

    public double? Pcent { get; set; }
}
