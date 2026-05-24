using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwTariffDummyForRpt
{
    public string ItemName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int Price { get; set; }

    public string Capitated { get; set; } = null!;

    public string Company { get; set; } = null!;
}
