using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class VwBillProcessReverseList2
{
    public string? CoyCode { get; set; }

    public string CoyName { get; set; } = null!;

    public bool? IsOld { get; set; }

    public string? BatchNo2 { get; set; }

    public string RetainCode { get; set; } = null!;

    public string? BatchVal { get; set; }

    public string? BillMonth { get; set; }

    public string? BillYear { get; set; }
}
