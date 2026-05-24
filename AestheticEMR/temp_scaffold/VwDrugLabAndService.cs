using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugLabAndService
{
    public string DrugService { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? Capitated { get; set; }

    public double? Private { get; set; }

    public double? Nhis { get; set; }

    public double? Hmo { get; set; }

    public double? HospCoy { get; set; }

    public double? _3mthly { get; set; }

    public double? _6mthly { get; set; }

    public double? Cbn { get; set; }

    public double? Nepa { get; set; }

    public string Remarks { get; set; } = null!;
}
