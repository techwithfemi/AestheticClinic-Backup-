using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillHeadingForDrug
{
    public string? RptHead { get; set; }

    public string Category { get; set; } = null!;

    public string ItemName { get; set; } = null!;
}
