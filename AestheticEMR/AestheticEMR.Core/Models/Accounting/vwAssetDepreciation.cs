using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwAssetDepreciation
{
    public long SNo { get; set; }

    public DateTime DeprDate { get; set; }

    public string AccountID { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public string? CalPeriod { get; set; }

    public string? Mth { get; set; }

    public decimal Amount { get; set; }

    public string? Yr { get; set; }

    public bool isPost { get; set; }

    public DateTime? EntryDate { get; set; }

    public DateTime? EntryTime { get; set; }

    public string? ClientName { get; set; }

    public string? AppName { get; set; }

    public string? AccountNoAccumDepr { get; set; }

    public bool? suppres { get; set; }

    public string? CanDepr { get; set; }
}
