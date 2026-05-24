using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HScreeningTest
{
    public long Sno { get; set; }

    public string ItemTest { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string? Remarks { get; set; }
}
