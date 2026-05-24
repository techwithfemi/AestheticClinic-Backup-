using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugNhislistEntered
{
    public string CoyId { get; set; } = null!;

    public string CoyName { get; set; } = null!;

    public string? Remarks { get; set; }

    public string Type { get; set; } = null!;
}
