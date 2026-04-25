using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Legacy;

public partial class DrugNHISListEntered
{
    public string CoyID { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Remarks { get; set; }

    public string? UseTariff { get; set; }

    public double? PCent { get; set; }
}
