using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HExpenseSetting
{
    public long Sno { get; set; }

    public string SetId { get; set; } = null!;

    public string SetName { get; set; } = null!;
}
