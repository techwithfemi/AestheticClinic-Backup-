using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugInteractionEnumAbmatch
{
    public string DrugA { get; set; } = null!;

    public string DrugB { get; set; } = null!;

    public string Remarks { get; set; } = null!;

    public string WarnLevel { get; set; } = null!;
}
