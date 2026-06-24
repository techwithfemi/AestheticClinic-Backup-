using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwAccountsInfoBalSheet2Temp
{
    public string GroupID { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public long GroupMin { get; set; }

    public long GroupMax { get; set; }

    public decimal? AccountOpAmt { get; set; }

    public decimal? AccountClAmt { get; set; }

    public string CatID { get; set; } = null!;

    public bool? HiddenGp { get; set; }

    public string CatMasterID { get; set; } = null!;

    public string CatMasterName { get; set; } = null!;

    public string CatName { get; set; } = null!;

    public string Period { get; set; } = null!;

    public string CoyID { get; set; } = null!;

    public int? Yr { get; set; }

    public int? MonthCounter { get; set; }
}
