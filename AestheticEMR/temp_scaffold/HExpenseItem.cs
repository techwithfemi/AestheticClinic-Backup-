using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class HExpenseItem
{
    public long Sno { get; set; }

    public string CatCode { get; set; } = null!;

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }
}
