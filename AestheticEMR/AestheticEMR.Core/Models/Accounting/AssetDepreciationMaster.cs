using System;
using System.Collections.Generic;

namespace AestheticEMR.Core.Models.Accounting;

public partial class AssetDepreciationMaster
{
    public long SNo { get; set; }

    public DateTime EntryDate { get; set; }

    public string AccountID { get; set; } = null!;

    public string AssetCode { get; set; } = null!;

    public DateTime AQuireDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? DisposalDate { get; set; }

    public DateTime? DateLastDepr { get; set; }

    public int DurationInMths { get; set; }

    public decimal GrossValue { get; set; }

    public decimal? DeprAmount { get; set; }

    public decimal? AccumDeprAmount { get; set; }

    public decimal? ScrapValue { get; set; }

    public int? DeprCount { get; set; }

    public bool? Active { get; set; }
}
