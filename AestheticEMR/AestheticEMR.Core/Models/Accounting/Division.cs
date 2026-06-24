using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class Division
{
    public long SNo { get; set; }

    public string? DivID { get; set; }

    public string DivName { get; set; } = null!;

    public string? Location { get; set; }

    public string? CoyID { get; set; }
}
