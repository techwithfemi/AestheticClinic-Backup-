using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class vwConfirmFinRpt_COA_2_XXXX
{
    public decimal? Asset { get; set; }

    public decimal? L { get; set; }

    public decimal? E { get; set; }

    public decimal? LE { get; set; }

    public decimal? MustBeZero { get; set; }

    public string CoyID { get; set; } = null!;

    public string Period { get; set; } = null!;
}
