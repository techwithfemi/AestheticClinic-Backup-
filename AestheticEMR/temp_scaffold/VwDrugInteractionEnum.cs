using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugInteractionEnum
{
    public long Sno { get; set; }

    public string DrugA { get; set; } = null!;

    public string DrugB { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string WarnLevel { get; set; } = null!;
}
