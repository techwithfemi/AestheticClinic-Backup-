using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillHeadingUnion
{
    public string? RptHead { get; set; }

    public string Category { get; set; } = null!;

    public string? ItemName { get; set; }

    public string BillType { get; set; } = null!;
}
