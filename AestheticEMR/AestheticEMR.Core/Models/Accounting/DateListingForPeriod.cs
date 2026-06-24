using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class DateListingForPeriod
{
    public long SNo { get; set; }

    public DateTime TranDate { get; set; }

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string? Remarks { get; set; }
}
