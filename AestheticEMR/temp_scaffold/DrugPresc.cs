using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class DrugPresc
{
    public long Sno { get; set; }

    public string Description { get; set; } = null!;

    public long? Qty { get; set; }
}
