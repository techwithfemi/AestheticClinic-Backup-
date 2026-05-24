using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugInteractionEnum
{
    public long Sno { get; set; }

    public string DrugA { get; set; } = null!;

    public string DrugB { get; set; } = null!;
}
