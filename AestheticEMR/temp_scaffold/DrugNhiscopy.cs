using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugNhiscopy
{
    public string DrgName { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string? Dosage { get; set; }

    public double Price { get; set; }

    public string? Company { get; set; }

    public string? Remarks { get; set; }
}
