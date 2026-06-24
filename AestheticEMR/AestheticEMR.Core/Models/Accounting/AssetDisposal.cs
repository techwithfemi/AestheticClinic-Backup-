using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetDisposal
{
    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string FormNo { get; set; } = null!;

    public DateTime Dates { get; set; }

    public string? DisposalReason { get; set; }

    public decimal ProfitLoss { get; set; }

    public string UselLife { get; set; } = null!;

    public decimal NetBookValue { get; set; }

    public decimal SalesValue { get; set; }
}
