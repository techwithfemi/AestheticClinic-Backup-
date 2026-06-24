using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetTransfer
{
    public string AssetCode { get; set; } = null!;

    public string AssetName { get; set; } = null!;

    public string FormNo { get; set; } = null!;

    public string FrmAssetDpt { get; set; } = null!;

    public string FrmAssetLcn { get; set; } = null!;

    public string FrmAssetGrp { get; set; } = null!;

    public string FrmAssetSbGrp { get; set; } = null!;

    public DateTime Dates { get; set; }

    public string ToAssetDpt { get; set; } = null!;

    public string ToAssetLcn { get; set; } = null!;

    public string ToAssetGrp { get; set; } = null!;

    public string ToAssetSbGrp { get; set; } = null!;

    public decimal NetBookValue { get; set; }

    public string? Reason { get; set; }

    public long SNo { get; set; }
}
