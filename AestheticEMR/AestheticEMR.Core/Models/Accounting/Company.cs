using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class Company
{
    public long SNo { get; set; }

    public string CoyID { get; set; } = null!;

    public string Coyname { get; set; } = null!;
}
