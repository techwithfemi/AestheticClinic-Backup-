using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwDrugItemsForStock
{
    public string Drgname { get; set; } = null!;

    public string DrgCatName { get; set; } = null!;

    public string? QtyPerUnit { get; set; }
}
