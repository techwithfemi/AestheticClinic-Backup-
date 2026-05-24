using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HInstantFee
{
    public long Sno { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal Amount { get; set; }

    public string ItemType { get; set; } = null!;
}
