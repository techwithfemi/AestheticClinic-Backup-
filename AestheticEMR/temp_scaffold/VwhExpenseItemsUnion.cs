using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwhExpenseItemsUnion
{
    public string ItemName { get; set; } = null!;

    public long ItemCode { get; set; }

    public string? CatCode { get; set; }
}
